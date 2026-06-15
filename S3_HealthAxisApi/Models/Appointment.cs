using System.ComponentModel.DataAnnotations;
using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        [Required]
        public DateOnly ScheduledDate { get; set; }

        [Required]
        public AppointmentTimeSlot TimeSlot { get; set; }

        public AppointmentStatus Status { get; set; }
            = AppointmentStatus.Pending;

        [StringLength(500)]
        public string? CancellationReason { get; set; }

        public HealthRecord? HealthRecord { get; set; }
    }
}