using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Metadata;

namespace HealthAxis.API.DTOs.Patients
{
    public class PatientCreateDto
    {
        [Required(ErrorMessage = Helpers.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Helpers.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.DateOfBirthRequired)]
        [DataType(DataType.Date)]
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
        [EmailAddress(ErrorMessage = Helpers.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email { get; set; } = string.Empty;
    }
}
