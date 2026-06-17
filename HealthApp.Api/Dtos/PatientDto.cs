using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class PatientDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(Male|Female|Other)$",
            ErrorMessage = "Gender must be Male, Female, or Other.")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Insurance ID cannot exceed 50 characters.")]
        public string InsuranceId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }
    }
}