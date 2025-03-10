using System.ComponentModel.DataAnnotations;

namespace WarehouseProject.Models.DTOs {
    public class ProductDTO {
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [MaxLength(1500, ErrorMessage = "Description cannot exceed 1500 characters.")]
        public string? Description { get; set; }

        [MaxLength(255, ErrorMessage = "Images URL cannot exceed 255 characters.")]
        //[Url(ErrorMessage = "Images must be a valid URL.")] 
        public string? Images { get; set; }

        [MaxLength(20, ErrorMessage = "Unit cannot exceed 20 characters.")]
        public string? Unit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int? Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available quantity must be a non-negative number.")]
        public int? AvailableQuantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 99999999.99, ErrorMessage = "Price must be between 0.01 and 99,999,999.99.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Cost price is required.")]
        [Range(0.01, 99999999.99, ErrorMessage = "Cost price must be between 0.01 and 99,999,999.99.")]
        public decimal CostPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive integer.")]
        public int? CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Supplier ID must be a positive integer.")]
        public int? SupplierId { get; set; }
    }
}