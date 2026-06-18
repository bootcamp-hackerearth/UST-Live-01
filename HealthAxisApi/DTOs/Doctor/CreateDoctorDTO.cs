using HealthAxisCore_Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.Doctor
{
    public class CreateDoctorDTO
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string DoctorName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public SpecialisationType Specialisation { get; set; }

        [Required]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Fee must be between 0 and 100000")]
        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}