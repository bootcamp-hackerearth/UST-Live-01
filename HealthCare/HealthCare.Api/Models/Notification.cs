using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public string? UserId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; }
    }
}
