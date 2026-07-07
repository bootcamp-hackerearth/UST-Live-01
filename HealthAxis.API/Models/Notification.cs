using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;

        [MaxLength(50)]
        public string NotificationType { get; set; } = "General";

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}