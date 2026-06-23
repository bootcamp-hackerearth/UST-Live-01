using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.Authentication
{
    public class LoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }= string.Empty;
    }
}
