using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseProject.Models
{
    public partial class Product
    {
        public Product()
        {
            InventoryHistories = new HashSet<InventoryHistory>();
            InventoryQuota = new HashSet<InventoryQuotum>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Images { get; set; }
        public string? Unit { get; set; }
        public int? Quantity { get; set; }
        public int? AvailableQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual Category? Category { get; set; }
        
        [JsonIgnore]
        public virtual Supplier? Supplier { get; set; }

        [JsonIgnore]
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; }

        [JsonIgnore]
        public virtual ICollection<InventoryQuotum> InventoryQuota { get; set; }
    }
}
