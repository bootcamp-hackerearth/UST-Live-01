using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Dto.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public string TimeSlot { get; set; } = string.Empty;
        [RegularExpression("(Pending|Confirmed|Completed|Cancelled)")]
        public string Status { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? CancellationReason { get; set; }
    }

    public class CreateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^\d{2}:\d{2}(-\d{2}:\d{2})?$")]
        public string TimeSlot { get; set; } = string.Empty;
    }


    public class UpdateAppointmentStatusDto
    {
        [Required]
        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$")]
        public string Status { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? CancellationReason { get; set; }
    }
}
