using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class CancelByDoctorDto
    {
        [Required(ErrorMessage = "Please select the doctor cancelling this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid doctor reference.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Please provide a reason for cancelling the appointment.")]
        [StringLength(500, ErrorMessage = "Cancellation reason must not exceed 500 characters.")]
        public string Reason { get; set; }
    }
}