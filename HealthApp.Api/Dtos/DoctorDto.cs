using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class DoctorDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\.\-]+$",
            ErrorMessage = "Doctor name can contain only letters, spaces, dot, and hyphen.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        [MinLength(2, ErrorMessage = "Specialisation must be at least 2 characters.")]
        [StringLength(50, ErrorMessage = "Specialisation cannot exceed 50 characters.")]
        public string Specialisation { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$",
            ErrorMessage = "Doctor phone number must be exactly 10 digits.")]
        public string DoctorPhoneNo { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid doctor email address.")]
        [StringLength(150, ErrorMessage = "Doctor email cannot exceed 150 characters.")]
        public string DoctorEmail { get; set; }

        [Required]
        [Range(0, 60, ErrorMessage = "Years of experience must be between 0 and 60.")]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
        public decimal ConsultationFee { get; set; }

        [Required(ErrorMessage = "Active status is required.")]
        public bool IsActive { get; set; }
    }
}
