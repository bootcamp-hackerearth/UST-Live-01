using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class Patient
    {
        [Key]
        public int PatientId
        {
            get;
            set;
        }

        [Required(ErrorMessage = Helpers.FullNameRequired)]
        [StringLength(ValidationLimits.FullNameLength)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = Helpers.InvalidFullNameFormat)]
        public string FullName
        {
            get;
            set;
        } = string.Empty;

        [Required(ErrorMessage = Helpers.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(
            typeof(Patient),
            nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth
        {
            get;
            set;
        }

        [Required(ErrorMessage = Helpers.GenderRequired)]
        public Gender Gender
        {
            get;
            set;
        }

        [Required(ErrorMessage = Helpers.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = Helpers.InvalidPhoneNumberFormat)]
        public string PhoneNumber
        {
            get;
            set;
        } = string.Empty;

        [Required(ErrorMessage = Helpers.EmailRequired)]
        [EmailAddress(ErrorMessage = Helpers.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email
        {
            get;
            set;
        } = string.Empty;

        public DateTime CreatedDate
        {
            get;
            set;
        } = DateTime.Now;

        public virtual ICollection<Appointment> Appointments
        {
            get;
            set;
        } = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords
        {
            get;
            set;
        } = new List<HealthRecord>();

        public int GetAge()
        {
            int age =
                DateTime.Today.Year - DateOfBirth.Year;

            if (DateOfBirth > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public static ValidationResult? ValidateDateOfBirth(
            DateTime date,
            ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult(
                    Helpers.DateOfBirthCannotBeFuture);
            }

            return ValidationResult.Success;
        }
    }
}
