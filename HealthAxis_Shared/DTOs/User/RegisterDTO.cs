using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.DTOs.User
{
    public class RegisterDTO
    {
        // ✅ Email
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        // ✅ Password
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        // ✅ Confirm Password
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        // ✅ Role (only "Patient" allowed now, validated in service)
        [Required]
        public string Role { get; set; } = null!;

        

        [Required]
        public string PatientName { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
    }
}
