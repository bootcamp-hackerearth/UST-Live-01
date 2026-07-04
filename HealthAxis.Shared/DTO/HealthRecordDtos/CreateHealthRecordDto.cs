using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.HealthRecordDtos
{
    public class CreateHealthRecordDto
    {
        [Required(ErrorMessage = "Appointment id is required")]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Visit date is required")]
        [CustomValidation(typeof(CreateHealthRecordDto), nameof(ValidateVisitDate))]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500, ErrorMessage = "Prescription cannot exceed 500 characters")]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string Notes { get; set; } = string.Empty;

        public static ValidationResult? ValidateVisitDate(
            DateTime date,
            ValidationContext context)
        {
            if (date == default)
            {
                return new ValidationResult("Visit date is required");
            }

            if (date.Date > DateTime.Today)
            {
                return new ValidationResult("Visit date cannot be in the future");
            }

            return ValidationResult.Success;
        }
    }
}