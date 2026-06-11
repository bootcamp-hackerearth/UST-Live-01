using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.Appointment
{
    public class UpdateAppointmentDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [RegularExpression("Confirmed|Pending|Cancelled|Completed",
            ErrorMessage = "Invalid status")]
        public string Status { get; set; }

        [StringLength(500)]
        public string CancellationReason { get; set; }
    }
}