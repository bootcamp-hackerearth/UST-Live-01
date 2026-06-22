using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.DTO.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor id is required")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Scheduled date is required")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required")]
        public string TimeSlot { get; set; } = string.Empty;
    }
}