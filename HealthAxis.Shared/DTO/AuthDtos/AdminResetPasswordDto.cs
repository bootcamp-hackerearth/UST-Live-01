using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AuthDtos
{
    public class AdminResetPasswordDto
    {
        public string? UserId { get; set; }

        public string? Email { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}