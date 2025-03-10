using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface ISupplierService {
        List<Supplier> GetAll(string? search);
        Supplier GetDetail(int id);
        (bool isSuccess, string message) Create(SupplierDTO entity);
        (bool isSuccess, string message) Update(int id, SupplierDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
