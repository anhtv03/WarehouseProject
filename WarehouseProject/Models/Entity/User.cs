using System;
using System.Collections.Generic;

namespace WarehouseProject.Models
{
    public partial class User
    {
        public User()
        {
            InventoryHistories = new HashSet<InventoryHistory>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Role? RoleNavigation { get; set; }
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; }
    }
}
