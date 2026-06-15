using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string TimeSlot { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } // "Confirmed" | "Pending" | "Cancelled" | "Completed"

        [MaxLength(500)]
        public string CancellationReason { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public virtual HealthRecord HealthRecord { get; set; }
    }
}
