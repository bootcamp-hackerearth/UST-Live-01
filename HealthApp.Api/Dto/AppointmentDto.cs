using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dto
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required]
        public int? PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string? CancellationReason { get; set; }
    }
}