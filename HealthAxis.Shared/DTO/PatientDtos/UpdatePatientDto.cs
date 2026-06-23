using HealthAxis.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.PatientDtos
{
    public class UpdatePatientDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Full name should contain only alphabets and spaces")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        [CustomValidation(typeof(UpdatePatientDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be valid 10 digit number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        public static ValidationResult? ValidateDateOfBirth(
            DateTime date,
            ValidationContext context)
        {
            if (date.Year < 1900)
            {
                return new ValidationResult("Date of birth year must be 1900 or later");
            }

            if (date > DateTime.Today)
            {
                return new ValidationResult("Date of birth cannot be in the future");
            }

            return ValidationResult.Success;
        }
    }
}