using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.Models
{
 
    public class Patient
    {

        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = Constants.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Constants.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Constants.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(
            typeof(Patient),
            nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = Constants.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Constants.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Constants.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = Constants.EmailRequired)]
        [EmailAddress(
            ErrorMessage = Constants.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        public virtual ICollection<Insurance> Insurances { get; set; }
            = new List<Insurance>();

        public int GetAge()
        {

            int age = DateTime.Today.Year - DateOfBirth.Year;

            if (DateOfBirth > DateTime.Today.AddYears(-age))
            {

                age--;
            }

            return age;
        }

        public static ValidationResult ValidateDateOfBirth(
            DateTime date,
            ValidationContext context)
        {

            if (date > DateTime.Today)
            {

                return new ValidationResult(
                    Constants.DateOfBirthCannotBeFuture);
            }

            return ValidationResult.Success;
        }
    }
}