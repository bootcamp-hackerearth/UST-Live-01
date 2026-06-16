using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.DTOs.Appointments
{
    public class UpdateAppointmentDto
    {
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = null!;

        [MaxLength(500)]
        public string? CancellationReason { get; set; }
    }
}
