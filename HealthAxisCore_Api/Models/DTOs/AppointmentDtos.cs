using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string CancellationReason { get; set; } = string.Empty;
    }

    public class CreateAppointmentDto
    {
        [JsonRequired]
        public int DoctorId { get; set; }

        [JsonRequired]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public required string TimeSlot { get; set; }
    }

    public class UpdateAppointmentStatusDto
    {
        [Required]
        [RegularExpression("(Pending|Confirmed|Cancelled|Completed)")]
        public required string Status { get; set; }

        [MaxLength(100)]
        public string CancellationReason { get; set; } = string.Empty;
    }
}