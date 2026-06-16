using HealthAxisCore_Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.Patient
{
    public class CreatePatientDTO
    {
        [Required]
        public string PatientName { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        public string? InsuranceID { get; set; }
    }
}