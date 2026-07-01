using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.DTOs;

public class ChangePasswordDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required, MinLength(6)]
    [RegularExpression(@"^[A-Z][a-z]+(?=.*\d)(?=.*[^a-zA-Z0-9]).*$", ErrorMessage = "Password must start with a capital letter followed by lowercase letters, and contain at least one number and one special character.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required, Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}
