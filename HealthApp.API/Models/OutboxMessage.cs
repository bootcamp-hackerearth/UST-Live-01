namespace HealthApp.API.Models;

public class OutboxMessage
{
    public Guid OutboxMessageId { get; set; }

    public string EventType { get; set; } = string.Empty;

    public string Payload { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public int RetryCount { get; set; }

    public DateTime? LastAttemptDate { get; set; }

    public string? ErrorMessage { get; set; }
}