using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } = null!;
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public string TimeSlot { get; set; } = string.Empty;
        [RegularExpression("(Pending|Confirmed|Completed|Cancelled)")]
        public string Status { get; set; } = "Pending";
        [MaxLength(200)]
        public string? CancellationReason { get; set; }
    }
}
