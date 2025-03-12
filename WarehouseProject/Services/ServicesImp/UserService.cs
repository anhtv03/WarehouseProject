using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using WarehouseProject.Models.DTOs;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Services.ServicesImp {
    public class UserService : IUserService {
        private readonly WarehouseDBContext _context;

        public UserService(WarehouseDBContext context) {
            _context = context;
        }

        //====================================================================================================
        public (bool isSuccess, string message) Create(RegisterDTO entity) {
            try {
                if (_context.Users.Any(p => p.Username == entity.Username)) {
                    return (false, "Username already exists.");
                }

                var user = new User {
                    Username = entity.Username,
                    FullName = entity.FullName,
                    Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                    Phone = entity.Phone,
                    Address = entity.Address,
                    Email = entity.Email,
                    Role = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };

                _context.Users.Add(user);
                _context.SaveChanges();
                return (true, "Register Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public (bool isSuccess, string message) Delete(int id) {
            try {
                var user = _context.Users.FirstOrDefault(p => p.UserId == id);

                if (user == null) {
                    return (false, "No found to delete.");
                }

                _context.Orders.Where(p => p.UserId == id).ToList().ForEach(x => x.UserId = null);
                //_context.Users.Remove(user);
                _context.SaveChanges();
                return (true, "Delete successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }

        public List<UserViewDTO> GetAll(string? search) {
            try {
                var query = _context.Users.Select(u => new UserViewDTO {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Email = u.Email,
                    Address = u.Address,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                }).AsQueryable();

                if (!string.IsNullOrEmpty(search)) {
                    query = query.Where(p => p.Email.Contains(search) ||
                                             p.Phone.Contains(search) ||
                                             p.Address.Contains(search) ||
                                             p.FullName.Contains(search));
                }

                query = query.OrderByDescending(p => p.CreatedAt);

                return query.ToList();
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public UserViewDTO GetDetail(int id) {
            try {
                var user = _context.Users.Select(u => new UserViewDTO {
                    UserId = u.UserId,
                    Username = u.Username,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Email = u.Email,
                    Address = u.Address,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                }).FirstOrDefault(p => p.UserId == id);
                return user;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Login(LoginDTO entity, HttpContext httpContext) {
            try {
                var user = _context.Users.FirstOrDefault(p => p.Username == entity.Username);

                if (user == null || !BCrypt.Net.BCrypt.Verify(entity.Password, user.Password)) {
                    return (false, "Wrong username or password");
                }

                var sessionUser = new UserViewDTO {
                    UserId = user.UserId,
                    Username = user.Username,
                    FullName = user.FullName,
                    Phone = user.Phone,
                    Email = user.Email,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                httpContext.Session.SetString("User", JsonSerializer.Serialize(sessionUser));

                return (true, JsonSerializer.Serialize(sessionUser));
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public (bool isSuccess, string message) Update(int id, UserDTO entity) {
            try {
                var user = _context.Users.FirstOrDefault(p => p.UserId == id);

                if (user == null) {
                    return (false, "No user found to update.");
                }

                user.FullName = entity.FullName;
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                return (true, "Update Successful");
            } catch (Exception ex) {
                return (false, ex.Message);
            }
        }
    }
}
