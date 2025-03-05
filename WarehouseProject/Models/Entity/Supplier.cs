using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseProject.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SupplierId { get; set; }
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
