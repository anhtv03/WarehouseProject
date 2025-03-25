using BCrypt.Net;
using WarehouseProject.Models;
using WarehouseProject.Models.Entity;

namespace WarehouseProject.Util {
    public class SeedData {
        private WarehouseDBContext _context;

        public SeedData(WarehouseDBContext context) {
            _context = context;
        }

        public void SeedRole() {
            if (!_context.Roles.Any()) {
                List<Role> roles = new List<Role> {
                        new Role {RoleName = "Admin" },
                        new Role {RoleName = "Staff" }
                    };

                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }
        }

        public void AddAdmin() {
            if (!_context.Users.Any()) {
                List<User> users = new List<User> {
                    new User {
                        Username = "admin",
                        Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                        FullName = "Admin",
                        Role = 1
                    },
                    new User {
                        Username = "staff01",
                        Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                        FullName = "Staff",
                        Address = "HCM",
                        Phone = "0123456789",
                        Email = "staff@gmail.com",
                    Role = 2
                    }
                };
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }

    }
}
