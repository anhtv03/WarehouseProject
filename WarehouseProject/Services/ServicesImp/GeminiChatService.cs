using GenerativeAI;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WarehouseProject.Models.DTOs;

namespace WarehouseProject.Services.ServicesImp {
    public class GeminiChatService : IGeminiChatService {

        private readonly GenerativeModel _model;
        private readonly IDistributedCache _cache;

        public GeminiChatService(string apiKey, IDistributedCache cache) {
            _model = new GenerativeModel(apiKey, "gemini-1.5-flash-latest");
            _cache = cache;
        }

        public async Task<ChatResponse> StartConversationAsync(string initialPrompt) {
            var conversationId = Guid.NewGuid().ToString();

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
            history.Add($"User: {userMessage}");

            var prompt = string.Join("\n", history);
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

        public async Task<string> GenerateTextAsync(string prompt) {
            try {
                var response = await _model.GenerateContentAsync(prompt);
                return response.Text ?? "Không có phản hồi";
            } catch (Exception ex) {
                return $"Lỗi: {ex.Message}";
            }
        }


    }
}
