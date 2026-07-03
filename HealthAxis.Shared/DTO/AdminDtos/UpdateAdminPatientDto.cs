using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class UpdateAdminPatientDto : IValidatableObject
    {
        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Patient name must be between 3 and 80 characters.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Patient name can contain only letters and spaces.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = "Phone number must be a valid 10-digit mobile number.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Address cannot exceed 150 characters.")]
        public string Address { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (DateOfBirth.Date > DateTime.Today)
            {
                yield return new ValidationResult(
                    "Date of birth cannot be in the future.",
                    new[] { nameof(DateOfBirth) });
            }

            if (DateOfBirth.Date < DateTime.Today.AddYears(-120))
            {
                yield return new ValidationResult(
                    "Date of birth is not valid.",
                    new[] { nameof(DateOfBirth) });
            }
        }
    }
}