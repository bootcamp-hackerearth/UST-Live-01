using HealthApp.Api.Enums;

namespace HealthApp.Api.Dtos
{
    using HealthApp.Api.Enums;
    using System.ComponentModel.DataAnnotations;

    public class DoctorCreateDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\.\-]+$",
            ErrorMessage = "Doctor name can contain only letters, spaces, dot, and hyphen.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required.")]
        public SpecialisationType? Specialisation { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Doctor phone number must be exactly 10 digits.")]
        public string DoctorPhoneNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid doctor email address.")]
        [StringLength(150, ErrorMessage = "Doctor email cannot exceed 150 characters.")]
        public string DoctorEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
        public decimal ConsultationFee { get; set; }
    }

}
