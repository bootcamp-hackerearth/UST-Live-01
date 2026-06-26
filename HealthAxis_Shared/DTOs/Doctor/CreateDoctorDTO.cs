using System.ComponentModel.DataAnnotations;
using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.DTOs.Doctor
{
    public class CreateDoctorDTO
    {
        [Required(ErrorMessage = "Doctor name is required")]
        [MinLength(2, ErrorMessage = "Doctor name must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Doctor name cannot exceed 100 characters")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Years of experience is required")]
        [Range(0, 60, ErrorMessage = "Experience must be between 0 and 60 years")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(0, 100000, ErrorMessage = "Fee must be between 0 and 100000")]
        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}