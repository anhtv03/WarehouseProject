using WarehouseProject.Models;

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

    }
}
