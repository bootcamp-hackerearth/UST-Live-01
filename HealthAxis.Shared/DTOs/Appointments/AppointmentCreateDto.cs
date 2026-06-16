using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace HealthAxis.API.DTOs.Appointments
{
    public class AppointmentCreateDto
    {
        [Required(ErrorMessage = Helpers.PatientRequired)]
        public int PatientId { get; set; }

        [Required(ErrorMessage = Helpers.DoctorRequired)]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = Helpers.ScheduledDateRequired)]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = Helpers.TimeSlotRequired)]
        [StringLength(ValidationLimits.TimeSlotLength)]
        public string TimeSlot { get; set; } = string.Empty;

        [Required(ErrorMessage = Helpers.AppointmentStatusRequired)]
        public AppointmentStatus Status { get; set; }
    }
}
