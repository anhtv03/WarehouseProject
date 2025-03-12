using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WarehouseProject.Models.Entity
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string Status { get; set; } = null!;
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? OrderType { get; set; }

        [JsonIgnore]
        public virtual Customer? Customer { get; set; }
        [JsonIgnore]
        public virtual Supplier? Supplier { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
