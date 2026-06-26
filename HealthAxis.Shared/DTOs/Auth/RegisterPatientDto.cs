using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTOs.Auth
{
    public class RegisterPatientDto
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

        [Required(ErrorMessage = Helpers.PasswordRequired)]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare(
            nameof(Password),
            ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
