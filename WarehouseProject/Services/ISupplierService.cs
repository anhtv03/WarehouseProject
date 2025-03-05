using WarehouseProject.Models.Form;
using WarehouseProject.Models;

namespace WarehouseProject.Services {
    public interface ISupplierService {
        List<Supplier> GetAll(string? search);
        Supplier GetDetail(int id);
        (bool isSuccess, string message) Create(SupplierDTO entity);
        (bool isSuccess, string message) Update(int id, SupplierDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
