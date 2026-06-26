using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTOs.Patients
{
    public class PatientUpdateDto
    {
        [Required(ErrorMessage = Helpers.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Helpers.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [DateOfBirthValidation(
            ErrorMessage = Helpers.InvalidDateOfBirthRange)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = Helpers.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Helpers.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Helpers.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.EmailRequired)]
        [StringLength(ValidationLimits.EmailLength)]
        [RegularExpression(
            RegexPatterns.StrictEmail,
            ErrorMessage = Helpers.InvalidStrictEmailFormat)]
        public string Email { get; set; } = string.Empty;
    }
}

