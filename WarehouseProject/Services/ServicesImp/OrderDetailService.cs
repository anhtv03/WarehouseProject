using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services.ServicesImp {
    public class OrderDetailService : IOrderDetailService {
        private readonly WarehouseDBContext _context;

        public OrderDetailService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================
        public (bool isSuccess, string message) Create(OrderDetailDTO entity) {
            try {
                if (entity.OrderId.HasValue && !_context.Orders.Any(c => c.OrderId == entity.OrderId)) {
                    return (false, "Order does not exist.");
                }
                if (entity.ProductId.HasValue && !_context.Products.Any(s => s.ProductId == entity.ProductId)) {
                    return (false, "Product does not exist.");
                }
                var price = _context.Products.FirstOrDefault(p => p.ProductId == entity.ProductId).Price;
                var ordd = new OrderDetail {
                    ProductId = entity.ProductId,
                    OrderId = entity.OrderId,
                    Quantity = entity.Quantity,
                    TotalPrice = entity.Quantity * price,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.OrderDetails.Add(ordd);
                _context.SaveChanges();
                return (true, "Created Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var order = _context.OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);

                if (order == null) {
                    return (false, "No found to delete.");
                }

                _context.OrderDetails.Remove(order);
                _context.SaveChanges();
                return (true, "Delete successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<OrderDetail> GetAll() {
            try {
                var list = _context.OrderDetails
                                   .OrderByDescending(p => p.CreatedAt)
                                   .Include(x => x.Order)
                                   .Include(x => x.Product)
                                   .ToList();
                return list;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public OrderDetail GetDetail(int id) {
            try {
                var order = _context.OrderDetails
                                    .OrderByDescending(p => p.CreatedAt)
                                    .Include(x => x.Order)
                                    .Include(x => x.Product)
                                    .FirstOrDefault(p => p.OrderDetailId == id);
                return order;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderDetail> GetByOrderId(int id) {
            try {
                var list = _context.OrderDetails
                                   .OrderByDescending(p => p.CreatedAt)
                                   .Where(p => p.OrderId == id)
                                   .Include(x => x.Order)
                                   .Include(x => x.Product)
                                   .ToList();
                return list;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<OrderDetail> GetByProductId(int id) {
            try {
                var list = _context.OrderDetails
                                   .OrderByDescending(p => p.CreatedAt)
                                   .Where(p => p.ProductId == id)
                                   .Include(x => x.Order)
                                   .Include(x => x.Product)
                                   .ToList();
                return list;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, OrderDetailDTO entity) {
            try {
                var order = _context.OrderDetails.FirstOrDefault(p => p.OrderDetailId == id);

                if (order == null) {
                    return (false, "No found to update.");
                }
                if (entity.OrderId.HasValue && !_context.Orders.Any(c => c.OrderId == entity.OrderId)) {
                    return (false, "Order does not exist.");
                }
                if (entity.ProductId.HasValue && !_context.Products.Any(s => s.ProductId == entity.ProductId)) {
                    return (false, "Product does not exist.");
                }

                var price = _context.Products.FirstOrDefault(p => p.ProductId == entity.ProductId).Price;
                order.ProductId = entity.ProductId;
                order.OrderId = entity.OrderId;
                order.Quantity = entity.Quantity;
                order.TotalPrice = entity.Quantity * price;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Updated Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}
