using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Dtos
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm new password is required.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}