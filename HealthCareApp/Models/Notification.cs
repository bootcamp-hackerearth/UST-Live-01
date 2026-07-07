using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Doctor? Doctor { get; set; }

        public Appointment? Appointment { get; set; }
    }
}
