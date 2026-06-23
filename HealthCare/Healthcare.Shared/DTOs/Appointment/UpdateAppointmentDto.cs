using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.Appointments
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
