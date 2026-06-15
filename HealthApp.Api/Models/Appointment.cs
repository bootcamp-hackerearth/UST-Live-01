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

        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; } 

        [Required(ErrorMessage = "Scheduled date is required.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        public string TimeSlot { get; set; }

        [Required(ErrorMessage = "Appointment status is required.")]
        [Range(0, 3, ErrorMessage = "Invalid appointment status.")]
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

        public string? CancellationReason { get; private set; }

    }
}
