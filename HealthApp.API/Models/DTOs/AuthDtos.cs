using System.ComponentModel.DataAnnotations;
using HealthApp.API.Enums;

namespace HealthApp.API.Models.DTOs;

public class RegisterRequestDto
{
    [Required] 
    public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] 
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] 
    public string Password { get; set; } = string.Empty;
    [Required] 
    public string Role { get; set; } = string.Empty; // Patient, Doctor, Admin
    public DateTime? DateOfBirth { get; set; }
    public GenderType? Gender { get; set; }
    public string? PhoneNumber { get; set; }
    public string? InsuranceId { get; set; }
}
public class LoginRequestDto { 
    [Required, EmailAddress] 
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
}
