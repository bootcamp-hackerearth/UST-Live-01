using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.API.DTOs.AppointmentDtos
{

    public class CreateAppointmentDto
    {

        #region Properties

        [Required(ErrorMessage = ValidationMessages.DoctorRequired)]
        public int DoctorId { get; set; }

        [Required(
            ErrorMessage = ValidationMessages.AppointmentDateRequired)]
        [DataType(DataType.Date)]
        [CustomValidation(
            typeof(Appointment),
            nameof(Appointment.ValidateScheduledDate))]
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
