using HealthCareApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        public NotificationType NotificationType { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }

        public Appointment? Appointment { get; set; }
    }
}