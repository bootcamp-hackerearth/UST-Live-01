using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(RegexPatterns.FullName, ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string TemporaryPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Range(ValidationLimits.MinExperience, ValidationLimits.MaxExperience,
            ErrorMessage = ValidationMessages.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Range(typeof(decimal), ValidationLimits.MinConsultationFee, ValidationLimits.MaxConsultationFee,
            ErrorMessage = ValidationMessages.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }
    }
}