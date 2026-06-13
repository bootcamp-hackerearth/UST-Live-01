using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class CancelByPatientDto
    {
        [Required(ErrorMessage = "Please select the patient cancelling this appointment.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid patient reference.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please provide a reason for cancelling the appointment.")]
        [StringLength(500, ErrorMessage = "Cancellation reason must not exceed 500 characters.")]
        public string Reason { get; set; }
    }
}