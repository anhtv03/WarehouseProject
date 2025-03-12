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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
                return (true, "Created Successful");
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
                    query = query.Where(p => p.Status.Contains(search));
                }

                query = query.Where(p => p.OrderType.Equals(role.ToString())).OrderByDescending(p => p.CreatedAt);

                return query.Include(x => x.User)
                            .Include(x => x.Customer)
                            .Include(x => x.Supplier)
                            .ToList();
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

                order.Status = entity.Status;
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
    }
}
