using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class ConfirmAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
    }
}