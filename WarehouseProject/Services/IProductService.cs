using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface IProductService {
        List<Product> GetAll(string? search);
        Product GetDetail(int id);
        List<Product> GetProductByCategory(int id);
        Task<(bool isSuccess, string message)> CreateAsync(ProductDTO entity, IFormFile? image);
        Task<(bool isSuccess, string message)> UpdateAsync(int id, ProductDTO entity, IFormFile? image);
        (bool isSuccess, string message) Delete(int id);
    }
}
