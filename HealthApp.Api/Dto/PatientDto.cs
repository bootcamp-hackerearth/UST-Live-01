using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dto
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required]
        [Range(3, 200)]
        public string? FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender specified.")]
        public string? Gender { get; set; }

        [MaxLength(20)]
        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+@[a-zA-Z0-9]+\.[a-zA-Z]$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? InsuranceId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
