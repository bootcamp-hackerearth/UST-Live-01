using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class RegisterPatientDto
    {
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+")]
        [MinLength(2)]
        public required string PatientName { get; set; }

        [JsonRequired]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("(Male|Female|Transgender|Other)")]
        public required string Gender { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        public required string RefreshToken { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public required string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }

        [Required]
        public required string ConfirmPassword { get; set; }
    }

    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public bool FirstLogin { get; set; }
    }

    public class ChangeFirstLoginPasswordDto
    {
        [Required]
        public required string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }

        [Required]
        public required string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }

    public class ForgotPasswordResponseDto
    {
        public string Message { get; set; } = string.Empty;

        public string ResetToken { get; set; } = string.Empty;
    }

    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        [MinLength(8)]
        public required string NewPassword { get; set; }
    }
}