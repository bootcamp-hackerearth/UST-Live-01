using HealthCareApp.AdminBlazor.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Patients
{
    public class CreatePatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public GenderType Gender { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string InsuranceId { get; set; } = string.Empty;
    }
}