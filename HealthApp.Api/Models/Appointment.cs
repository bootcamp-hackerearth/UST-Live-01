using HealthApp.Api.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models
{

    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient? Patient { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; } 

        [Required]
        public DateOnly ScheduledDate { get; set; }

        [Required]
        public string? TimeSlot { get; set; }

        [Required]
        [Range(0, 3, ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string? CancellationReason { get; set; }

    }
}
