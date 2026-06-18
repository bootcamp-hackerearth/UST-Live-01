using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTO.PatientDtos
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(RegexPatterns.FullName, ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        public string? FullName { get; set; }

        [Required(ErrorMessage = ValidationMessages.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreatePatientDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth{ get; set; }

        [Required(ErrorMessage = ValidationMessages.GenderRequired)]
        public Gender Gender { get;set; }

        [Required( ErrorMessage = ValidationMessages.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(RegexPatterns.PhoneNumber, ErrorMessage = ValidationMessages.InvalidPhoneNumberFormat)]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string? Email { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult(ValidationMessages.DateOfBirthCannotBeFuture);
            }
            return ValidationResult.Success;
        }
    }
}