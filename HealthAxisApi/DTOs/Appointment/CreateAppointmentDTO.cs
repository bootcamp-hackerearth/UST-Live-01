using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.Appointment
{
    public class CreateAppointmentDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; } = null!;
    }
}
