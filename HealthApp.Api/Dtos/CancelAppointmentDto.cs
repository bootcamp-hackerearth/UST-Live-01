using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class CancelAppointmentDto
    {
        [Required(ErrorMessage = "Cancellation reason is required.")]
        [StringLength(250, ErrorMessage = "Cancellation reason cannot exceed 250 characters.")]
        public string CancellationReason { get; set; } = string.Empty;
    }
}