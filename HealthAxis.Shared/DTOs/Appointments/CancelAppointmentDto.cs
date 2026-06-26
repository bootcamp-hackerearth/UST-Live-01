using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTOs.Appointments
{
    public class CancelAppointmentDto
    {
        
        [StringLength(500)]
        public string CancellationReason { get; set; } = string.Empty;
    }
}
