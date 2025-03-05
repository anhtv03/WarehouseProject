using System;
using System.Collections.Generic;

namespace WarehouseProject.Models.Entity {
    public partial class Order {
        public Order() {
            InventoryHistories = new HashSet<InventoryHistory>();
        }

        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderType { get; set; }
        public int? Status { get; set; }
        public int UserId { get; set; }
        public int? ShippingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Shipping? Shipping { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<InventoryHistory> InventoryHistories { get; set; }
    }
}
