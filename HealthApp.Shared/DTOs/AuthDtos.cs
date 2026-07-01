using System.ComponentModel.DataAnnotations;
using HealthApp.Shared.Enums;

namespace HealthApp.Shared.DTOs;

public class RegisterRequestDto
{
    [Required]
    [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Full name must start with an uppercase letter and be at least 3 characters long.")]
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress(ErrorMessage = "Invalid email address.")] 
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [RegularExpression(@"^[A-Z][a-z]+(?=.*\d)(?=.*[^a-zA-Z0-9]).*$", ErrorMessage = "Password must start with a capital letter followed by lowercase letters, and contain at least one number and one special character.")]
    public string Password { get; set; } = string.Empty;
    [Required] 
    public DateTime? DateOfBirth { get; set; }
    public GenderType? Gender { get; set; }
    [Required]
    [RegularExpression(@"[6-9]\d{9}", ErrorMessage = "Invalid phone number.")]
    public string? PhoneNumber { get; set; }
    public string? InsuranceId { get; set; }
}
public class LoginRequestDto { 
    [Required, EmailAddress(ErrorMessage = "Invalid email address.")] 
    public string Email { get; set; } = string.Empty; 
    [Required] 
    public string Password { get; set; } = string.Empty; }

public class RefreshTokenRequestDto { 
    [Required] 
    public string AccessToken { get; set; } = string.Empty; 
    [Required] 
    public string RefreshToken { get; set; } = string.Empty; }
public class AuthResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
    public bool MustChangePassword { get; set; }

    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
}
