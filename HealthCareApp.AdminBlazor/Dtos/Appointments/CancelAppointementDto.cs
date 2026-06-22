using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Appointments
{
    public class CancelAppointmentDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}