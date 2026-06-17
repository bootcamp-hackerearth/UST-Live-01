using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.AuthDtos
{
    [ExcludeFromCodeCoverage]
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
