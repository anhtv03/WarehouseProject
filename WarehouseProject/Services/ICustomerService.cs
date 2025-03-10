using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface ICustomerService {
        List<Customer> GetAll(string? search);
        Customer GetDetail(int id);
        (bool isSuccess, string message) Create(CustomerDTO entity);
        (bool isSuccess, string message) Update(int id, CustomerDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
