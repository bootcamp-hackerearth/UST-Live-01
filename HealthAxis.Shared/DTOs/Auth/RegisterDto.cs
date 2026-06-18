using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public int ReferenceId { get; set; }
    }
}
