using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class CompleteAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
    }
}