using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string RecipientUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NotificationType { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        public int? RelatedEntityId { get; set; }

        [StringLength(100)]
        public string? RelatedEntityType { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}