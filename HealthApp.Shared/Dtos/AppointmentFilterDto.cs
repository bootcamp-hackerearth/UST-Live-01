using HealthApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dtos
{
    public class AppointmentFilterDto : IValidatableObject
    {
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid positive number.")]
        public int? DoctorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a valid positive number.")]
        public int? PatientId { get; set; }

        [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus? Status { get; set; }

        public DateOnly? Date { get; set; }

        public DateOnly? FromDate { get; set; }

        public DateOnly? ToDate { get; set; }

        public bool OnlyUpcoming { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0.")]
        public int PageNumber { get; set; } = 1;
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date.HasValue && (FromDate.HasValue || ToDate.HasValue))
            {
                yield return new ValidationResult(
                    "Use either exact date or date range, not both.",
                    new[] { nameof(Date), nameof(FromDate), nameof(ToDate) });
            }

            if (FromDate.HasValue && ToDate.HasValue && FromDate.Value > ToDate.Value)
            {
                yield return new ValidationResult(
                    "From date cannot be greater than to date.",
                    new[] { nameof(FromDate), nameof(ToDate) });
            }
        }
    }
}