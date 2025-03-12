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
                User user = new User {
                    Username = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    FullName = "Admin",
                    Role = 1
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
        }

    }
}
