using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class ConfirmAppointmentDto
    {
        [Required(ErrorMessage = "Please select the doctor confirming this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }
    }
}