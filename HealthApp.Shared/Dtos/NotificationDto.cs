namespace HealthApp.Shared.Dtos
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }

        public string NotificationType { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public int? RelatedEntityId { get; set; }

        public string? RelatedEntityType { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
