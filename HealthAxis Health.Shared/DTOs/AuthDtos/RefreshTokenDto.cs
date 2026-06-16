using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.Shared.DTOs.AuthDtos
{
    public class RefreshTokenDto
    {
        #region Properties

        [Required(
            ErrorMessage = ValidationMessages.RefreshTokenRequired)]
        [StringLength(
            ValidationLimits.RefreshTokenLength)]
        public string RefreshToken { get; set; } = string.Empty;

        #endregion
    }
}
