using HealthCareApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareApp.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = null!;

        [Required]
        public AppointmentStatus Status { get; set; }

        [MaxLength(200)]
        public string? CancellationReason { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public HealthRecord? HealthRecord { get; set; }
    }
}