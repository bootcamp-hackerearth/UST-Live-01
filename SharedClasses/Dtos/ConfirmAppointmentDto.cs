using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class ConfirmAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }
    }
}