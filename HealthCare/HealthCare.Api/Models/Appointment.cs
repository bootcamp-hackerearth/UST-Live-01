using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class Appointment
    {
       
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateOnly ScheduledDate { get; set; }

        [Required]
        [MaxLength(20)]
        public string TimeSlot { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [AllowedValues("Pending", "Confirmed", "Cancelled", "Completed", ErrorMessage = "Status should be Pending,Confirmed,Concelled or Completed")]
        public string Status { get; set; } = "Pending";

        [MaxLength(500)]
        public string CancellationReason { get; set; }

        public DateTimeOffset CreatedDate { get; set; }= DateTimeOffset.UtcNow;

        // Navigation
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } = null!;

        public HealthRecord HealthRecord { get; set; }
    }
}
