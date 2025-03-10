using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface IOrderService {
        List<Order> GetAll(string? search);
        Order GetDetail(int id);
        (bool isSuccess, string message) Create(OrderDTO entity);
        (bool isSuccess, string message) Update(int id, OrderDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
