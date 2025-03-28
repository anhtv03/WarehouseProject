﻿using System.ComponentModel.DataAnnotations;

namespace WarehouseFrontEnd.Models.DTOs {
    public class RegisterDTO {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string? FullName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(10, ErrorMessage = "Phone number cannot exceed 10 characters.")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string? Email { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public int? Role { get; set; }

    }
}
