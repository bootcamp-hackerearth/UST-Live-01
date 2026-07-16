using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Model
{
    public class Notification
    {
        [Key]
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        [Required]
        public string Message { get; set; } = string.Empty;
        [Required]
        public string EventType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
