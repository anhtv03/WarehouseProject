using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services {
    public interface IUserService {
        List<UserViewDTO> GetAll(string? search);
        UserViewDTO GetDetail(int id);
        (bool isSuccess, string message) Login(LoginDTO entity, HttpContext httpContext);
        (bool isSuccess, string message) Create(RegisterDTO entity);
        (bool isSuccess, string message) Update(int id, UserDTO entity);
        (bool isSuccess, string message) Delete(int id);
    }
}
