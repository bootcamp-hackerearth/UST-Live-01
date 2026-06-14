using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis_Health.API.DTOs.AuthDtos
{
    public class RegisterDto
    {

        #region Properties

        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [StringLength(
            ValidationLimits.FullNameLength,
            ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(
            typeof(Patient),
            nameof(Patient.ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = ValidationMessages.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = ValidationMessages.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = ValidationMessages.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

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