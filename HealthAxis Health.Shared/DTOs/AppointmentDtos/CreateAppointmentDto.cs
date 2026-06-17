using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.AppointmentDtos
{

    [ExcludeFromCodeCoverage]
    public class CreateAppointmentDto
    {

        #region Properties

        [Required(ErrorMessage = ValidationMessages.DoctorRequired)]
        public int DoctorId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.AppointmentDateRequired)]
        [CustomValidation(
            typeof(CustomValidators),
            nameof(CustomValidators.ValidateScheduledDate))]
        public DateTime ScheduledDate { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.TimeSlotRequired)]
        [StringLength(
            ValidationLimits.TimeSlotLength,
            ErrorMessage = ValidationMessages.InvalidTimeSlot)]
        public string TimeSlot { get; set; } = string.Empty;

        #endregion
    }
}
