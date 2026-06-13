using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class CancelAppointmentDto
    {
        [Required(ErrorMessage = "Please provide a reason for cancelling the appointment.")]
        [StringLength(500, ErrorMessage = "Cancellation reason must not exceed 500 characters.")]
        public string Reason { get; set; }
    }
}