using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dto
{
    public class PatientDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender specified.")]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? InsuranceId { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(450)]
        public string? IdentityUserId { get; set; }
    }
}