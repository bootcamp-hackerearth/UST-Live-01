using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public required string TimeSlot { get; set; }
        [RegularExpression("(Pending|Confirmed|Cancelled|Completed)")]
        public required string Status { get; set; }
        [MaxLength(100)]
        public required string CancellationReason { get; set; }
    }
}
