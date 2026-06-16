using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.Shared.DTOs.PatientDtos
{
    public class UpdatePatientDto
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
        [CustomValidation(
            typeof(CustomValidators),
             nameof(CustomValidators.ValidateDateOfBirth))]
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

        #endregion
    }
}
