using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(6, ErrorMessage = "New password must contain at least 6 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}