namespace HealthCareApp.Models
{
    public class OutboxMessage
    {
        public int OutboxMessageId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public int RetryCount { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public DateTime? PublishedDateUtc { get; set; }

        public string? ErrorMessage { get; set; }
    }
}