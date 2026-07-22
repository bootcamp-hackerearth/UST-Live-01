using HealthAxisCore_Api.Constants;

namespace HealthAxisCore_Api.Models
{
    public sealed class OutboxMessage
    {
        public long OutboxMessageId { get; set; }

        public Guid EventId { get; set; }

        public string EventType { get; set; } =
            string.Empty;

        public string Payload { get; set; } =
            string.Empty;

        public string Status { get; set; } =
            OutboxMessageStatuses.Pending;

        public int RetryCount { get; set; }

        public DateTime CreatedDate { get; set; } =
            DateTime.UtcNow;

        public DateTime? PublishedDate { get; set; }

        public DateTime? LastAttemptDate { get; set; }

        public DateTime? NextRetryDate { get; set; }

        public string? ErrorMessage { get; set; }
    }
}