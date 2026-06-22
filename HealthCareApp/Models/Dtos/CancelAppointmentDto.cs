using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Dtos
{
    public class CancelAppointmentDto
    {

        [Required]
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Please provide a reason for cancelling the appointment.")]
        [StringLength(500, ErrorMessage = "Cancellation reason must not exceed 500 characters.")]
        public required string Reason { get; set; }
    }
}