using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Dto.Appointments
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CancellationReason { get; set; }
    }

    public class CreateAppointmentDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$",
         ErrorMessage = "Time must be in HH:mm format (00:00–23:59)")]
        public string TimeSlot { get; set; } = string.Empty;
    }


    public class UpdateAppointmentStatusDto
    {

        [Required]
        [RegularExpression("^(Pending|Confirmed|Cancelled|Completed)$",
            ErrorMessage = "Invalid status value")]
        public string Status { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? CancellationReason { get; set; }
    }
}
