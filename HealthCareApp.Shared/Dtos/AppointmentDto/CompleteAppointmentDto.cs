using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Shared.Dtos.Appointments
{
    public class CompleteAppointmentDto
    {
        [Required(ErrorMessage = "Please select the doctor completing this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }
    }
}