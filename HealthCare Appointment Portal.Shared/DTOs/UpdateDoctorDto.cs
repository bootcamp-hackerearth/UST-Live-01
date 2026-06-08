using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.DoctorDtos
{
    public class UpdateDoctorDto
    {
        [Required(
            ErrorMessage = Constants.FullNameRequired)]
        [StringLength(
            ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName
        {
            get;
            set;
        }

        [Required(
            ErrorMessage = Constants.SpecialisationRequired)]
        public Specialisation Specialisation
        {
            get;
            set;
        }

        [Range(
            ValidationLimits.MinExperience,
            ValidationLimits.MaxExperience,
            ErrorMessage = Constants.InvalidExperienceRange)]
        public int YearsOfExperience
        {
            get;
            set;
        }

        [Range(
            typeof(decimal),
            ValidationLimits.MinConsultationFee,
            ValidationLimits.MaxConsultationFee,
            ErrorMessage = Constants.InvalidConsultationFee)]
        public decimal ConsultationFee
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
    }
}