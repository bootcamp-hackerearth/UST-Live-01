using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.AdminBlazor.Dtos.Appointments
{
    public class BookAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string TimeSlot { get; set; } = string.Empty;
    }
}