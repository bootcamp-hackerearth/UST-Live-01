using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Role { get; set; } = "Patient";

        public int ReferenceId { get; set; }
    }
}
