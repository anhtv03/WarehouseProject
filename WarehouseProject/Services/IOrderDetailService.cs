using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface IOrderDetailService {
        List<OrderDetail> GetAll();
        List<OrderDetail> GetByOrderId(int id);
        List<OrderDetail> GetByProductId(int id);
        OrderDetail GetDetail(int id);
        (bool isSuccess, string message) Create(OrderDetailDTO entity);
        (bool isSuccess, string message) Update(int id, OrderDetailDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
