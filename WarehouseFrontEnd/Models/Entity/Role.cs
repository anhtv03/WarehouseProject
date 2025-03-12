using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseFrontEnd.Models.Entity
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
