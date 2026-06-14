using HealthAxisHealth.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.API.DTOs.AuthDtos
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
