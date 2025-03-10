using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface IProductService {
        List<Product> GetAll(string? search);
        Product GetDetail(int id);
        List<Product> GetProductByCategory(int id);
        (bool isSuccess, string message) Create(ProductDTO entity);
        (bool isSuccess, string message) Update(int id, ProductDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
