using HealthCareApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Dtos
{
    public class DoctorRegistrationRequestDto
    {
        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, ErrorMessage = "Doctor name must not exceed 100 characters.")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Practice start date is required.")]
        public DateTime PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100,000.")]
        public decimal ConsultationFee { get; set; }
    }
}