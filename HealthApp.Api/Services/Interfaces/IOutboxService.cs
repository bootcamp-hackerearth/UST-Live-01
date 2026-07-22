using HealthApp.Api.Models;
using HealthApp.Shared.Events;

namespace HealthApp.Api.Services.Interfaces;

public interface IOutboxService
{
    Task<Guid> EnqueueAppointmentBookedAsync(
        AppointmentBookedEvent message,
        CancellationToken ct = default);

    Task<List<OutboxMessage>> GetEligibleMessagesAsync(
        int maximumRetryCount,
        int batchSize,
        TimeSpan processingTimeout,
        CancellationToken ct = default);

    Task MarkProcessingAsync(
        OutboxMessage message,
        CancellationToken ct = default);

    Task MarkPublishedAsync(
        OutboxMessage message,
        CancellationToken ct = default);

    Task MarkFailedAsync(
        OutboxMessage message,
        Exception exception,
        CancellationToken ct = default);
}
