using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace HealthAxis.API.Models
{

    public class Patient
    {

        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = Constant.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Constant.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = Constant.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(
            typeof(Patient),
            nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = Constant.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Constant.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Constant.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = Constant.EmailRequired)]
        [EmailAddress(
            ErrorMessage = Constant.InvalidEmailFormat)]
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
                    Constant.DateOfBirthCannotBeFuture);
            }

            return ValidationResult.Success;
        }
    }
}