using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTO.DoctorDtos
{
    public class ChangePasswordDto
    {
        
            [Required(ErrorMessage = "Current password is required")]
            public string CurrentPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "New password is required")]
            [MinLength(8, ErrorMessage = "New password must be at least 8 characters")]
            [RegularExpression(
                @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
                ErrorMessage = "New password must contain uppercase letter, number and special character")]
            public string NewPassword { get; set; } = string.Empty;

            [Required(ErrorMessage = "Confirm password is required")]
            [Compare("NewPassword", ErrorMessage = "New password and confirm password do not match")]
            public string ConfirmNewPassword { get; set; } = string.Empty;
        }
    }


