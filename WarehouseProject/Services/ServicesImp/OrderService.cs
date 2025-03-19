using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;
using WarehouseProject.Util;

namespace WarehouseProject.Services.ServicesImp {
    public class OrderService : IOrderService {
        private readonly WarehouseDBContext _context;

        public OrderService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================
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

    }
}
