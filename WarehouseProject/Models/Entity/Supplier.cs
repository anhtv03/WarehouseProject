using System;
using System.Collections.Generic;

namespace WarehouseProject.Models.Entity {
    public partial class Supplier {
        public Supplier() {
            Orders = new HashSet<Order>();
        }

        public int SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
