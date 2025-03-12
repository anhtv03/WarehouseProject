using System.ComponentModel.DataAnnotations;
using WarehouseProject.Util;

namespace WarehouseProject.Models.DTOs {
    public class OrderDTO {
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "User ID must be a positive integer.")]
        public int? UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Customer ID must be a positive integer.")]
        public int? CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a positive integer.")]
        public int? SupplierId { get; set; }

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string? Note { get; set; }

        [EnumDataType(typeof(OrderTypeEnum), ErrorMessage = "Order type must be either 'Inbound' or 'Outbound'.")]
        public string? OrderType { get; set; }

    }
}