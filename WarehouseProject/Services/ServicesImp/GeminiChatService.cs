using GenerativeAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services.ServicesImp {
    public class GeminiChatService : IGeminiChatService {

        private readonly GenerativeModel _model;
        private readonly IDistributedCache _cache;
        private readonly WarehouseDBContext _context;

        public GeminiChatService(string apiKey, IDistributedCache cache, WarehouseDBContext context) {
            _model = new GenerativeModel(apiKey, "gemini-1.5-flash-latest");
            _cache = cache;
            _context = context;
        }

        public async Task<ChatResponse> StartConversationAsync(string initialPrompt) {
            var conversationId = Guid.NewGuid().ToString();
            var systemContext = await GetSystemContextAsync();
            var prompt = $"Bạn là một trợ lý AI chỉ được phép trả lời dựa trên dữ liệu hệ thống dưới đây. Không sử dụng bất kỳ thông tin nào ngoài dữ liệu này.\n\n{systemContext}\n\nCâu hỏi: {initialPrompt}";

            var response = await _model.GenerateContentAsync(initialPrompt);

            var history = new List<string> { $"User: {initialPrompt}", $"Assistant: {response.Text}" };
            var cacheEntryOptions = new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync(conversationId, JsonSerializer.Serialize(history), cacheEntryOptions);
            return new ChatResponse {
                ConversationId = conversationId,
                Response = response.Text ?? "Không có phản hồi"
            };
        }

        public async Task<ChatResponse> ContinueConversationAsync(string conversationId, string userMessage) {
            var historyJson = await _cache.GetStringAsync(conversationId);
            if (string.IsNullOrEmpty(historyJson)) {
                throw new ArgumentException("ConversationId không tồn tại hoặc đã hết hạn.");
            }

            var history = JsonSerializer.Deserialize<List<string>>(historyJson);
            var systemContext = await GetSystemContextAsync();
            history.Add($"User: {userMessage}");

            var prompt = $"Bạn là một trợ lý AI chỉ được phép trả lời dựa trên dữ liệu hệ thống dưới đây. Không sử dụng bất kỳ thông tin nào ngoài dữ liệu này.\n\n{systemContext}\n\nLịch sử trò chuyện:\n{string.Join("\n", history)}\n\nCâu hỏi: {userMessage}";

            var response = await _model.GenerateContentAsync(prompt);

            history.Add($"Assistant: {response.Text}");

            var cacheEntryOptions = new DistributedCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync(conversationId, JsonSerializer.Serialize(history), cacheEntryOptions);

            return new ChatResponse {
                ConversationId = conversationId,
                Response = response.Text ?? "Không có phản hồi"
            };
        }

        //==========================================================================================
        public async Task<string> GetSystemContextAsync() {
            var products = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.Supplier)
                .Select(p =>
                    $"Product: {p.Name}, Price: {p.Price}, QuanlityProductInStock: {p.Quantity}, CreateAt: {p.CreatedAt}, " +
                    $"Category: {p.Category.Name}, Supplier: {p.Supplier.Name}, SupplierAddress: {p.Supplier.Address}"
                )
                .ToListAsync();

            var orderData = await _context.Orders
                    .Include(x => x.User)
                    .Include(x => x.Customer)
                    .Include(x => x.Supplier)
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .Select(o => new {
                        o.Code,
                        o.CreatedAt,
                        o.OrderType,
                        CustomerFullName = o.Customer.FullName,
                        CustomerAddress = o.Customer.Address,
                        SupplierName = o.Supplier.Name,
                        SupplierAddress = o.Supplier.Address,
                        StaffFullName = o.User.FullName,
                        OrderDetails = o.OrderDetails.Select(od => new {
                            od.Product.Name,
                            od.Quantity,
                            od.TotalPrice,
                            od.CreatedAt
                        }).ToList()
                    })
                    .ToListAsync();

            var orders = orderData.Select(o => {
                string orderDetailsSummary = $"OrderDetails: {string.Join(", ", o.OrderDetails.Select(od =>
                    $"Product: {od.Name}, QuanlityProductInStock: {od.Quantity}, TotalPrice: {od.TotalPrice}, CreateAt: {od.CreatedAt}"
                ))}";
                return
                    $"Order: {o.Code}, OrderDate: {o.CreatedAt}, OrderType: {o.OrderType}, " +
                    $"Customer: {o.CustomerFullName}, CustomerAddress: {o.CustomerAddress} " +
                    $"Supplier: {o.SupplierName}, SupplierAddress: {o.SupplierAddress} " +
                    $"Staff: {o.StaffFullName}, " +
                    orderDetailsSummary;
            }).ToList();

            var context = new List<string>();
            context.Add("Dữ liệu hệ thống:");
            context.AddRange(products);
            context.AddRange(orders);

            return string.Join("\n", context);
        }

    }
}
