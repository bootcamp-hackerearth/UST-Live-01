using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;
namespace HealthAxis.API.DTO.AuthDtos
{

    public class RegisterDto
    {
        [Required(ErrorMessage = "Full name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Name should contain only alphabets and spaces")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [CustomValidation(typeof(RegisterDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Phone number must be valid 10 digit Indian number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Only Gmail address is allowed")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).+$",
            ErrorMessage = "Password must contain uppercase, lowercase and number")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

       

        public static ValidationResult? ValidateDateOfBirth(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult(
                    "Date of birth cannot be in the future");
            }

            if (date.Year < 1900)
            {
                return new ValidationResult(
                    "Date of birth year must be 1900 or later");
            }

            return ValidationResult.Success;
        }
    }
}