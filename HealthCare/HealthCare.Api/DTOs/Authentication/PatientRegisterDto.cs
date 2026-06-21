using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.DTOs.Authentication
{
    public class PatientRegisterDto
    {
        [Required]
        public string? FullName { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [RegularExpression("Male|Female|Other,ErrorMessage = \"Gender must be Male, Female, or Other.")]
        public string? Gender { get; set; }

        [Required]
        [Phone]
        [RegularExpression(@"^[6789]\d{9}$", ErrorMessage = "PhoneNumber must start with 6,7,8,9 and be only 10 digit long")]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string? ConfirmPassword { get; set; }


        [MaxLength(50)]
        public string? InsuranceId { get; set; }
    }
}
