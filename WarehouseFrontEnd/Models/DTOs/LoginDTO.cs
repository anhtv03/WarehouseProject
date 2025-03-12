using System.ComponentModel.DataAnnotations;

namespace WarehouseFrontEnd.Models.DTOs {
    public class LoginDTO {

        [Required(ErrorMessage = "Email not blank")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password not blank")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
