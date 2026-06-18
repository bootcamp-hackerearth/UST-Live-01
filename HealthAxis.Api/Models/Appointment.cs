using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisCore_Api.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [ForeignKey("PId")]
        public int PatientId { get; set; }
        public required Patient Patient { get; set; }
        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        public required Doctor Doctor { get; set; }
        [Required]
        public DateTime ScheduledDate { get; set; }
        [Required]
        public required string  TimeSlot { get; set; }
        [RegularExpression("(Pending|Confirmed|Cancelled|Completed)")]
        public required string Status { get; set; }
        [MaxLength(100)]
        public required string CancellationReason { get; set; }
    }
}
