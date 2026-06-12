using System;
using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.DTOs.PatientDtos
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = Constants.FullNameRequired)]
        public string FullName { get; set; }

        [Required(ErrorMessage = Constants.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }   // ✅ IMPORTANT CHANGE

        [Required(ErrorMessage = Constants.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Constants.PhoneNumberRequired)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = Constants.EmailRequired)]
        public string Email { get; set; }

        // ✅ VALIDATION FIX
        public static ValidationResult ValidateDateOfBirth(DateTime? date, ValidationContext context)
        {
            if (date == null)
                return new ValidationResult("Date Of Birth is required");

            if (date > DateTime.Today)
                return new ValidationResult("Date cannot be future");

            return ValidationResult.Success;
        }
    }
}