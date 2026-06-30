using HealthAxis.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.DoctorDtos
{
    public sealed class CreateDoctorDto
    {
        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Doctor name must be between 3 and 80 characters.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Doctor name can contain only letters and spaces.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        public Specialisation Specialisation { get; set; }

        [Range(0, 70, ErrorMessage = "Experience must be between 0 and 70 years.")]
        public int YearsOfExperience { get; set; }

        [Range(typeof(decimal), "1", "100000", ErrorMessage = "Consultation fee must be between ₹1 and ₹100000.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}