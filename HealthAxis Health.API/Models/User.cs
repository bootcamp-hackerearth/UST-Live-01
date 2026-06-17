using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.API.Models
{
    [ExcludeFromCodeCoverage]
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        #region Properties

        [Key]
        public int UserId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(
            ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [StringLength(
            ValidationLimits.EmailLength)]
        public string Email { get; set; } = string.Empty;

        [Required(
            ErrorMessage = ValidationMessages.PasswordRequired)]
        [StringLength(
            ValidationLimits.PasswordMaxLength,
            MinimumLength = ValidationLimits.PasswordMinLength)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        [StringLength(
            ValidationLimits.RefreshTokenLength)]
        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryDate { get; set; }
       

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate {  get; set; } = DateTime.UtcNow;

        #endregion

        #region Navigation Properties

        public virtual Patient? Patient { get; set; }

        public virtual Doctor? Doctor {  get; set; }
        

        #endregion

        #region Business Methods

        public bool IsRefreshTokenValid()
        {

            return !string.IsNullOrWhiteSpace(
                       RefreshToken)
                   &&
                   RefreshTokenExpiryDate.HasValue
                   &&
                   RefreshTokenExpiryDate >
                   DateTime.UtcNow;
        }

        public void SetRefreshToken(
            string refreshToken,
            DateTime expiryDate)
        {

            RefreshToken =
                refreshToken;

            RefreshTokenExpiryDate =
                expiryDate;
        }

        public void RevokeRefreshToken()
        {

            RefreshToken = null;

            RefreshTokenExpiryDate = null;
        }

        public void Activate()
        {

            IsActive = true;
        }

        public void Deactivate()
        {

            IsActive = false;
        }

        #endregion
    }
}
