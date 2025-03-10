using System.ComponentModel.DataAnnotations;

namespace WarehouseProject.Models.DTOs {

    public class CategoryDTO {

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
