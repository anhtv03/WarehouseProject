using WarehouseProject.Models;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface ICategoryService {
        List<Category> GetAll(string? search);
        Category GetDetail(int id);
        (bool isSuccess, string message) Create(CategoryDTO entity);
        (bool isSuccess, string message) Update(int id, CategoryDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
