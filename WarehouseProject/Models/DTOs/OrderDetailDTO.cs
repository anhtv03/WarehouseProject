using System.ComponentModel.DataAnnotations;

namespace WarehouseProject.Models.DTOs {
    public class OrderDetailDTO {
        [Required(ErrorMessage = "Order ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
        public int? OrderId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product ID must be a positive integer.")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }

        public string? orderType { get; set; }
    }
}