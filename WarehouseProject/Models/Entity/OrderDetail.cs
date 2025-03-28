﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WarehouseProject.Models.Entity
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
