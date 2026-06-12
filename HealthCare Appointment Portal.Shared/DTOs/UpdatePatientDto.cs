using System;
using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.DTOs.PatientDtos
{
    public class UpdatePatientDto
    {
        [Required(ErrorMessage = Constants.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName { get; set; }

        [Required(ErrorMessage = Constants.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }   // ✅ FIXED (IMPORTANT)

        [Required(ErrorMessage = Constants.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Constants.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Constants.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Constants.EmailRequired)]
        [EmailAddress(ErrorMessage = Constants.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email { get; set; }

        
    }
}