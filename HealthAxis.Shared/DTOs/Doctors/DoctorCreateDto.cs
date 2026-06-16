using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace HealthAxis.API.DTOs.Doctors
{
    public class DoctorCreateDto
    {
        [Required(ErrorMessage = Helpers.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Helpers.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = Helpers.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = Helpers.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
