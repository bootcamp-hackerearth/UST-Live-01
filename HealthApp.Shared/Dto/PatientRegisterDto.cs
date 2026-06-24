using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto

{
    public class PatientRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender specified.")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? InsuranceId { get; set; }
    }
}