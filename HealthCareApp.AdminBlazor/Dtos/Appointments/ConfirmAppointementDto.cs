using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Appointments
{
    public class ConfirmAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
    }
}