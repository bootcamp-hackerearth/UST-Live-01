using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Model
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
        public DateTime ScheduledDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(500)]
        public string? CancellationReason { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient? Patient { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor? Doctor { get; set; }
    }
}