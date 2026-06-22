using System.ComponentModel.DataAnnotations;
using S3_HealthAxis.Shared.Enums;

namespace S3_HealthAxis.Shared.DTOs.Auth
{
    public class RegisterPatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? InsuranceNumber { get; set; }
    }
}