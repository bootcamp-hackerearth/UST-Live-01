using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models;

[Table("OutboxMessages")]
public class OutboxMessage
{
    [Key]
    public long OutboxMessageId { get; set; }

    public Guid EventId { get; set; }

    [Required]
    [MaxLength(200)]
    public string EventType { get; set; } = string.Empty;

    [Required]
    public string Payload { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;

    public int RetryCount { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? LastAttemptAtUtc { get; set; }

    public DateTime? NextAttemptAtUtc { get; set; }

    public DateTime? PublishedAtUtc { get; set; }

    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }
}
