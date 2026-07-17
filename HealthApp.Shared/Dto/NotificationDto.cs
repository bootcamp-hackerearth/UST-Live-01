namespace HealthApp.Shared.Dto
{
    public class NotificationDto
    {
        public string NotificationId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string EventType { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}