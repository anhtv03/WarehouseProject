using System;
using System.Collections.Generic;

namespace WarehouseProject.Models.Entity {
    public partial class InventoryQuotum {
        public int QuotaId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
