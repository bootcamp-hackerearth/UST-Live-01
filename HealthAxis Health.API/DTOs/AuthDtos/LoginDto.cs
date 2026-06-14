using HealthAxisHealth.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.API.DTOs.AuthDtos
{

    public class LoginDto
    {

        #region Properties

        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(
            ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
        [StringLength(
            ValidationLimits.PasswordMaxLength,
            MinimumLength = ValidationLimits.PasswordMinLength)]
        public string Password { get; set; } = string.Empty;

        #endregion
    }
}
