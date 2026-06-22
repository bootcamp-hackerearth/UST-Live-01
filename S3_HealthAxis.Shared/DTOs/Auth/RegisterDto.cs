using System.ComponentModel.DataAnnotations;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxis.Shared.Enums;

namespace S3_HealthAxis.Shared.DTOs.Auth
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

        [Required]
        public UserRole Role { get; set; }

        public int? ReferenceId { get; set; }
    }
}