using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.HealthRecord
{
    public class CreateHealthRecordDto
    {
        [Required]
        public required int AppointmentId { get; set; }

        [Required]
        public required int PatientId { get; set; }

        [Required]
        [PastOrTodayDateValidationAttribute]
        public required DateTime VisitDate { get; set; }

        [Required]
        [MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Diagnosis must contain only letters and numbers.")]
        public string Diagnosis { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Prescription must contain only letters and numbers.")]
        public string Prescription { get; set; } = null!;

        [MaxLength(1000)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Notes must contain only letters and numbers.")]
        public string? Notes { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastOrTodayDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime visitDate && visitDate.Date > DateTime.Today)
            {
                return new ValidationResult("Visit date must be today or in the past.");
            }

            return ValidationResult.Success;
        }
    }
}
