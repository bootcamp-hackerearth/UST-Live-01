using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Doctors
{
    public class UpdateDoctorDto
    {
        [Required(ErrorMessage = "Doctor name is required.")]
        [MinLength(2, ErrorMessage = "Doctor name must contain at least 2 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required.")]
        public string Specialisation { get; set; } = string.Empty;

        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        [Range(1, 100000, ErrorMessage = "Consultation fee must be greater than 0.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}