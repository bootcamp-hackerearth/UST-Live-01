using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class RegisterPatientDto
    {
        [Required, RegularExpression(@"[A-Z][A-za-z\s]+"), MinLength(2)]
        public string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, RegularExpression("(Male|Female|Transgender|Other)")]
        public string Gender { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }
    }

    public class LoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
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
    }

    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
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
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}