namespace HealthCareApp.Models
{
    public class OutboxMessage
    {
        public Guid OutboxMessageId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public DateTime? PublishedDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public int RetryCount { get; set; }

        public string? ErrorMessage { get; set; }
    }
}