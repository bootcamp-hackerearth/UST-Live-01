using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.PatientDtos
{
    public class CreatePatientDto
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

        [Required(ErrorMessage = Constants.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1900-01-01", "2100-01-01",
    ErrorMessage = "Date must be valid")]
        [CustomValidation(typeof(CreatePatientDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(
            ErrorMessage = Constants.GenderRequired)]
        public Gender Gender
        {
            get;
            set;
        }

        [Required(
            ErrorMessage = Constants.PhoneNumberRequired)]
        [StringLength(
            ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Constants.InvalidPhoneNumberFormat)]
        public string PhoneNumber
        {
            get;
            set;
        }

        [Required(
            ErrorMessage = Constants.EmailRequired)]
        [EmailAddress(
            ErrorMessage = Constants.InvalidEmailFormat)]
        [StringLength(
            ValidationLimits.EmailLength)]
        public string Email
        {
            get;
            set;
        }

        // ✅ FIXED VALIDATION
        public static ValidationResult ValidateDateOfBirth(
    DateTime date,
    ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult("Date of Birth cannot be in future");
            }

            if (date.Year < 1900)
            {
                return new ValidationResult("Date of Birth year must be greater than 1900");
            }

            return ValidationResult.Success;
        }
    }
}
