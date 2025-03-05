using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;

namespace WarehouseProject.Services.ServicesImp {
    public class ProductService : IProductService {
        private readonly WarehouseDBContext _context;

        public ProductService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================

        public (bool isSuccess, string message) Create(ProductDTO entity) {
            try {

                if (entity.CategoryId.HasValue && !_context.Categories.Any(c => c.CategoryId == entity.CategoryId)) {
                    return (false, "Category ID does not exist.");
                }
                if (entity.SupplierId.HasValue && !_context.Suppliers.Any(s => s.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier ID does not exist.");
                }

                var product = new Product {
                    Name = entity.Name,
                    Description = entity.Description,
                    Images = entity.Images,
                    Unit = entity.Unit,
                    Quantity = entity.Quantity ?? 0,
                    AvailableQuantity = entity.AvailableQuantity ?? 0,
                    Price = entity.Price,
                    CostPrice = entity.CostPrice,
                    CategoryId = entity.CategoryId,
                    SupplierId = entity.SupplierId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Products.Add(product);
                _context.SaveChanges();
                return (true, "Create Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

                if (product == null) {
                    return (false, "No found to delete.");
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                return (true, "Delete successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<Product> GetAll(string? search) {
            try {
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(p => p.Name.Contains(search) ||
                                             p.Category.Name.Contains(search));
                }

                query = query.OrderByDescending(p => p.CreatedAt);

                return query.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Product GetDetail(int id) {
            try {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                return product;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<Product> GetProductByCategory(int id) {
            try {
                var products = _context.Products
                    .Where(p => p.CategoryId == id)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToList();
                return products;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, ProductDTO entity) {
            try {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

                if (product == null) {
                    return (false, "No product found to update.");
                }

                if (entity.CategoryId.HasValue && !_context.Categories.Any(c => c.CategoryId == entity.CategoryId)) {
                    return (false, "Category ID does not exist.");
                }
                if (entity.SupplierId.HasValue && !_context.Suppliers.Any(s => s.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier ID does not exist.");
                }

                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Images = entity.Images;
                product.Unit = entity.Unit;
                product.Quantity = entity.Quantity ?? product.Quantity;
                product.AvailableQuantity = entity.AvailableQuantity ?? product.AvailableQuantity;
                product.Price = entity.Price;
                product.CostPrice = entity.CostPrice;
                product.CategoryId = entity.CategoryId;
                product.SupplierId = entity.SupplierId;
                product.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}