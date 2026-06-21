using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models.DTOs;

public class ChangePasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required, Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
