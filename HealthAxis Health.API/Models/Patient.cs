using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisHealth.API.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Patient
    {

        #region Properties

        [Key]
        public int PatientId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
        [StringLength(
            ValidationLimits.FullNameLength,
            ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        [RegularExpression(
            RegexPatterns.FullName,
            ErrorMessage = ValidationMessages.InvalidFullNameFormat)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.DateOfBirthRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Patient), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = ValidationMessages.GenderRequired)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = ValidationMessages.PhoneNumberRequired)]
        [StringLength(ValidationLimits.PhoneNumberLength)]
        [RegularExpression(
            RegexPatterns.PhoneNumber,
            ErrorMessage = ValidationMessages.InvalidPhoneNumberFormat)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [StringLength(ValidationLimits.EmailLength)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
            = DateTime.UtcNow;

        #endregion

        #region Navigation Properties

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Appointment> Appointments { get; set; }
            = new List<Appointment>();

        public virtual ICollection<HealthRecord> HealthRecords { get; set; }
            = new List<HealthRecord>();

        #endregion

        #region Methods

        public int GetAge()
        {
            return CalculateAge(DateOfBirth);
        }

        #endregion

        #region Custom Validations

        public static ValidationResult? ValidateDateOfBirth(
             DateTime dateOfBirth,
            ValidationContext validationContext)
        {
            if (dateOfBirth.Date > DateTime.Today)
            {
                return new ValidationResult(
                    ValidationMessages.DateOfBirthCannotBeFuture);
            }

            if (CalculateAge(dateOfBirth) > ValidationLimits.MaximumAge)
            {
                return new ValidationResult(
                    ValidationMessages.InvalidAge);
            }

            return ValidationResult.Success;
        }

        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth > DateTime.Today.AddYears(-age))
            {

                age--;
            }
            return age;
        }

        #endregion
    }
}
