using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;
using System.Text;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace WarehouseProject.Services.ServicesImp {
    public class ProductService : IProductService {
        private readonly WarehouseDBContext _context;
        private readonly Cloudinary _cloudinary;

        public ProductService(WarehouseDBContext context, Cloudinary cloudinary) {
            _context = context;
            _cloudinary = cloudinary;
        }

        //====================================================================================================

        public async Task<(bool isSuccess, string message)> CreateAsync(ProductDTO entity, IFormFile? image) {
            try {
                if (entity.CategoryId.HasValue && !_context.Categories.Any(c => c.CategoryId == entity.CategoryId)) {
                    return (false, "Category does not exist.");
                }
                if (entity.SupplierId.HasValue && !_context.Suppliers.Any(s => s.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier does not exist.");
                }
                string? imageUrl = null;
                if (image != null) {
                    imageUrl = await UploadImageAsync(image);
                    if (imageUrl == null) {
                        return (false, "Image upload failed.");
                    }
                }

                var product = new Product {
                    Name = entity.Name,
                    Description = entity.Description,
                    Images = imageUrl,
                    Unit = entity.Unit,
                    Quantity = entity.Quantity ?? 0,
                    AvailableQuantity = entity.AvailableQuantity ?? 0,
                    Price = entity.Price,
                    CostPrice = entity.CostPrice,
                    IsActive = true,
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

                //product.IsActive = false;

                _context.OrderDetails.RemoveRange(_context.OrderDetails.Where(p => p.ProductId == id));
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

                return query.Include(x => x.Category)
                            .Include(x => x.Supplier)
                            .ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Product GetDetail(int id) {
            try {
                var product = _context.Products
                                    .Include(x => x.Category)
                                    .Include(x => x.Supplier)
                                    .FirstOrDefault(p => p.ProductId == id);
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

        public async Task<(bool isSuccess, string message)> UpdateAsync(int id, ProductDTO entity, IFormFile? image) {
            try {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null) {
                    return (false, "No product found to update.");
                }

                if (entity.CategoryId.HasValue && !await _context.Categories.AnyAsync(c => c.CategoryId == entity.CategoryId)) {
                    return (false, "Category does not exist.");
                }
                if (entity.SupplierId.HasValue && !await _context.Suppliers.AnyAsync(s => s.SupplierId == entity.SupplierId)) {
                    return (false, "Supplier does not exist.");
                }
                string? imageUrl = null;
                if (image != null) {
                    imageUrl = await UploadImageAsync(image);
                    if (imageUrl == null) {
                        return (false, "Image upload failed.");
                    }
                    if (!string.IsNullOrEmpty(product.Images)) {
                        await DeleteImageAsync(product.Images);
                    }
                }

                product.Name = entity.Name;
                product.Description = entity.Description;
                product.Images = imageUrl == null ? entity.Images : imageUrl;
                product.Unit = entity.Unit;
                product.Quantity = entity.Quantity ?? product.Quantity;
                product.AvailableQuantity = entity.AvailableQuantity ?? product.AvailableQuantity;
                product.Price = entity.Price;
                product.CostPrice = entity.CostPrice;
                product.CategoryId = entity.CategoryId;
                product.SupplierId = entity.SupplierId;
                product.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        //============================================================================
        public async Task<string?> UploadImageAsync(IFormFile image) {
            long maxFileSize = 5 * 1024 * 1024;

            if (image == null || image.Length == 0) {
                return null;
            }

            if (image.Length > maxFileSize) {
                return null;
            }

            try {
                using (var stream = image.OpenReadStream()) {
                    var uploadParams = new ImageUploadParams() {
                        File = new FileDescription(image.FileName, stream),
                        UploadPreset = "wmproject_img_products",
                        UseFilename = true,
                        UniqueFilename = true,
                        Folder = "WarehouseProject"
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return uploadResult.SecureUrl.ToString();
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private async Task DeleteImageAsync(string imageUrl) {
            try {
                var uri = new Uri(imageUrl);
                var pathSegments = uri.AbsolutePath.Split('/');
                var folderAndFileName = string.Join("/", pathSegments.Skip(pathSegments.Length - 2));
                var publicId = folderAndFileName.Substring(0, folderAndFileName.LastIndexOf('.'));

                var deletionParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deletionParams);

                if (result.Result != "ok") {
                    Console.WriteLine($"Failed to delete image: {result.Error?.Message}");
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error deleting image: {ex.Message}");
            }
        }

    }
}