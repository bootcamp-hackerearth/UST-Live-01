using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.Shared.DTOs.DoctorDtos
{
    public class UpdateDoctorDto
    {
        #region Properties

        [Required(ErrorMessage = ValidationMessages.DoctorNameRequired)]
        [StringLength(
            ValidationLimits.DoctorNameLength,
            ErrorMessage = ValidationMessages.InvalidDoctorNameFormat)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = ValidationMessages.InvalidDoctorNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.SpecialisationRequired)]
        public Specialisation Specialisation { get; set; }

        [Required(ErrorMessage = ValidationMessages.ExperienceRequired)]
        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = ValidationMessages.InvalidExperienceRange)]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = ValidationMessages.ConsultationFeeRequired)]
        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = ValidationMessages.InvalidConsultationFee)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }
}
