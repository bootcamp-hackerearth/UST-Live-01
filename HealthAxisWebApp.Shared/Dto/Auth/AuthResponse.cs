using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Patient|Doctor|Admin)$")]
        public string Role { get; set; } = "Patient";

        public string? Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNo { get; set; }

        public string? InsuranceID { get; set; }
    }


    public class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }
        public bool IsFirstLogin { get; set; }
    }


    public class AuthRegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }


    public class ChangePasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;
    }


}
