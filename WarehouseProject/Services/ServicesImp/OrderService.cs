using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Drawing;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;
using WarehouseProject.Util;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;

namespace WarehouseProject.Services.ServicesImp {
    public class OrderService : IOrderService {
        private readonly WarehouseDBContext _context;

        public OrderService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================
        public (bool isSuccess, string message, byte[] fileBytes, string fileName) ExportToExcel(string orderType) {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage()) {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách đơn hàng");

                using (var range = worksheet.Cells["A1:H1"]) {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                worksheet.Cells[1, 1].Value = "Mã Đơn";
                worksheet.Cells[1, 2].Value = "Ngày Khởi Tạo";
                worksheet.Cells[1, 3].Value = "Loại Đơn";
                worksheet.Cells[1, 4].Value = "Trạng Thái";
                if (orderType.Equals("Outbound")) {
                    worksheet.Cells[1, 5].Value = "Khách hàng";
                } else {
                    worksheet.Cells[1, 5].Value = "Nhà cung cấp";
                }
                worksheet.Cells[1, 6].Value = "Số Lượng Đặt";
                worksheet.Cells[1, 7].Value = "Tên Sản Phẩm";
                worksheet.Cells[1, 8].Value = "Giá Trị Đơn";

                var outboundOrders = _context.Orders
                                        .Include(o => o.Customer)
                                        .Include(o => o.Supplier)
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .Where(o => o.OrderType == orderType)
                                        .OrderByDescending(o => o.CreatedAt)
                                        .ToList();

                int row = 2;
                foreach (var order in outboundOrders) {
                    worksheet.Cells[row, 1].Value = order.Code;
                    worksheet.Cells[row, 2].Value = order.CreatedAt.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 3].Value = order.OrderType == "Outbound" ? "Đơn Xuất" : "Đơn Nhập";

                    worksheet.Cells[row, 4].Value = GetStatusText(order.Status);
                    if (order.Status == "cancel") {
                        worksheet.Cells[row, 4].Style.Font.Color.SetColor(Color.Red);
                    } else if (order.Status == "processed") {
                        worksheet.Cells[row, 4].Style.Font.Color.SetColor(Color.Green);
                    } else {
                        worksheet.Cells[row, 4].Style.Font.Color.SetColor(Color.Orange);
                    }

                    var totalCost = order.OrderDetails.Where(od => od.OrderId == order.OrderId)
                                                      .Sum(od => od.TotalPrice);
                    var totalQuantity = order.OrderDetails.Where(od => od.OrderId == order.OrderId)
                                                           .Select(od => od.ProductId)
                                                           .Distinct()
                                                           .Count();
                    var productName = string.Join(", ", order.OrderDetails
                                            .Where(od => od.OrderId == order.OrderId && od.Product != null)
                                            .Select(od => od.Product.Name)
                                            .Distinct()
                                            .ToList());

                    if (orderType.Equals("Outbound")) {
                        worksheet.Cells[row, 5].Value = order.Customer?.FullName ?? "Chưa có khách hàng";
                    } else {
                        worksheet.Cells[row, 5].Value = order.Supplier?.Name ?? "Chưa có nhà cung cấp";
                    }
                    worksheet.Cells[row, 6].Value = totalQuantity;
                    worksheet.Cells[row, 7].Value = productName;
                    worksheet.Cells[row, 8].Value = totalCost;
                    worksheet.Cells[row, 8].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    row++;
                }
                worksheet.Cells.AutoFitColumns();

                using (var range = worksheet.Cells[1, 1, row - 1, 8]) {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                string fileName = $"DanhSachDonHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var fileBytes = package.GetAsByteArray();
                return (true, "Export successful", fileBytes, fileName);
            }
        }

        public (bool isSuccess, string message) Create(OrderDTO entity) {
            try {
                if (entity.SupplierId.HasValue && !_context.Suppliers.Any(c => c.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier does not exist.");
                }
                if (entity.UserId.HasValue && !_context.Users.Any(s => s.UserId == entity.UserId)) {
                    return (false, "User does not exist.");
                }
                if (entity.CustomerId.HasValue && !_context.Customers.Any(s => s.CustomerId == entity.CustomerId)) {
                    return (false, "Customer does not exist.");
                }

                var order = new Order {
                    Status = "pending",
                    UserId = entity.UserId,
                    CustomerId = entity.CustomerId,
                    SupplierId = entity.SupplierId,
                    OrderDate = DateTime.Now,
                    Note = entity.Note,
                    OrderType = entity.OrderType,
                    Code = "IG" + 5000,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                order.Code = "IG" + (5000 + order.OrderId);
                _context.SaveChanges();
                return (true, order.OrderId.ToString());
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);

                if (order == null) {
                    return (false, "No found to delete.");
                }

                _context.OrderDetails.RemoveRange(_context.OrderDetails.Where(p => p.OrderId == id));
                _context.Orders.Remove(order);
                _context.SaveChanges();
                return (true, "Delete successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<Order> GetAll(OrderTypeEnum role, string? search) {
            try {
                var query = _context.Orders.AsQueryable();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(p => p.Code.Contains(search) ||
                                             p.Status.Contains(search));
                }

                query = query.Where(p => p.OrderType.Equals(role.ToString())).OrderByDescending(p => p.CreatedAt);

                var list = query.Include(x => x.User)
                                .Include(x => x.Customer)
                                .Include(x => x.Supplier)
                                .Include(x => x.OrderDetails)
                                .ThenInclude(p => p.Product)
                                .ToList();
                return list;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Order GetDetail(int id) {
            try {
                var order = _context.Orders
                                    .Include(x => x.User)
                                    .Include(x => x.Customer)
                                    .Include(x => x.Supplier)
                                    .Include(x => x.OrderDetails)
                                    .ThenInclude(p => p.Product)
                                    .FirstOrDefault(p => p.OrderId == id);
                return order;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, OrderDTO entity) {
            try {
                var order = _context.Orders.FirstOrDefault(p => p.OrderId == id);

                if (order == null) {
                    return (false, "No order found to update.");
                }

                if (entity.SupplierId.HasValue && !_context.Suppliers.Any(c => c.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier does not exist.");
                }
                if (entity.UserId.HasValue && !_context.Users.Any(s => s.UserId == entity.UserId)) {
                    return (false, "User does not exist.");
                }
                if (entity.CustomerId.HasValue && !_context.Customers.Any(s => s.CustomerId == entity.CustomerId)) {
                    return (false, "Customer does not exist.");
                }

                order.Status = "pending";
                order.CustomerId = entity.CustomerId;
                order.SupplierId = entity.SupplierId;
                order.Note = entity.Note;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) UpdateStatus(int id, string status) {
            try {
                var order = _context.Orders
                            .Include(o => o.OrderDetails)
                            .FirstOrDefault(p => p.OrderId == id);

                if (order == null) {
                    return (false, "No order found to update.");
                }
                if (!status.Equals("processed") && !status.Equals("cancel")) {
                    return (false, "Invalid status.");
                }
                if (order.Status == "cancel") {
                    return (false, "Order has been canceled.");
                }
                if (order.Status == "processed") {
                    return (false, "Order has been processed.");
                }
                if (order.OrderDetails == null || !order.OrderDetails.Any()) {
                    return (false, "Order has no details to process.");
                }

                if (status == "processed") {
                    var productIds = order.OrderDetails.Select(od => od.ProductId).ToList();
                    var productsToUpdate = _context.Products
                                            .Where(p => productIds.Contains(p.ProductId))
                                            .ToList();
                    foreach (var item in order.OrderDetails) {
                        var product = productsToUpdate.FirstOrDefault(p => p.ProductId == item.ProductId);
                        if (product == null) {
                            return (false, $"Product with ID {item.ProductId} not found.");
                        }

                        if (order.OrderType.Equals("Inbound")) {
                            product.Quantity += item.Quantity;
                        } else if (order.OrderType.Equals("Outbound")) {
                            if (product.Quantity < item.Quantity) {
                                return (false, $"Không đủ hàng cho sản phẩm {product.Name}. Có sẵn: {product.Quantity}, Yêu cầu: {item.Quantity}");
                            }
                            product.Quantity -= item.Quantity;
                        }
                    }
                }

                order.Status = status;
                order.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        private string GetStatusText(string status) {
            return status switch {
                "cancel" => "Đã hủy",
                "processed" => "Đã xử lý",
                _ => "Đang chờ xử lý"
            };
        }
    }
}
