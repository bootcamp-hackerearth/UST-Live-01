using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class CancelAppointmentDto
    {
        [Required]
        public string CancellationReason { get; set; }
    }

    public class UpdateAppointmentStatusDto
    {
        [Required]
        public AppointmentStatus Status { get; set; }
    }
}