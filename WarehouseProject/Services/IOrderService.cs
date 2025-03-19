using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;
using WarehouseProject.Util;

namespace WarehouseProject.Services {
    public interface IOrderService {
        List<Order> GetAll(OrderTypeEnum role, string? search);
        Order GetDetail(int id);
        (bool isSuccess, string message) Create(OrderDTO entity);
        (bool isSuccess, string message) Update(int id, OrderDTO entity);
        (bool isSuccess, string message) UpdateStatus(int id, string status);
        (bool isSuccess, string message) Delete(int id);
    }
}
