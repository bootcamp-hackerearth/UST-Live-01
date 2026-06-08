using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class CancelAppointmentDto
    {
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
    }
}