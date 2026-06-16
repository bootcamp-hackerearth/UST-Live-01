using System.ComponentModel.DataAnnotations;
using S3_HealthAxisApi.Enums;

namespace S3_HealthAxisApi.DTOs.Patient
{
    public class CreatePatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }
    }
}