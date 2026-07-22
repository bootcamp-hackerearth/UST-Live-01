using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Events;
using System.Text.Json;

namespace HealthApp.Api.Services.Impl;

public class OutboxService : IOutboxService
{
    private const int MaximumErrorMessageLength = 2000;

    private readonly IOutboxRepository _outboxRepository;

    public OutboxService(IOutboxRepository outboxRepository)
    {
        _outboxRepository = outboxRepository;
    }

    public async Task<Guid> EnqueueAppointmentBookedAsync(
        AppointmentBookedEvent message,
        CancellationToken ct = default)
    {
        var eventId = Guid.NewGuid();
        var nowUtc = DateTime.UtcNow;

        var outboxMessage = new OutboxMessage
        {
            EventId = eventId,
            EventType = OutboxEventTypes.AppointmentBooked,
            Payload = JsonSerializer.Serialize(message),
            Status = OutboxMessageStatuses.Pending,
            RetryCount = 0,
            CreatedAtUtc = nowUtc,
            NextAttemptAtUtc = nowUtc
        };

        await _outboxRepository.AddAsync(outboxMessage, ct);
        await _outboxRepository.SaveChangesAsync(ct);

        return eventId;
    }

    public Task<List<OutboxMessage>> GetEligibleMessagesAsync(
        int maximumRetryCount,
        int batchSize,
        TimeSpan processingTimeout,
        CancellationToken ct = default)
    {
        var nowUtc = DateTime.UtcNow;
        var staleProcessingCutoffUtc = nowUtc - processingTimeout;

        return _outboxRepository.GetEligibleMessagesAsync(
            nowUtc,
            staleProcessingCutoffUtc,
            maximumRetryCount,
            batchSize,
            ct);
    }

    public async Task MarkProcessingAsync(
        OutboxMessage message,
        CancellationToken ct = default)
    {
        message.Status = OutboxMessageStatuses.Processing;
        message.LastAttemptAtUtc = DateTime.UtcNow;
        message.ErrorMessage = null;

        await _outboxRepository.SaveChangesAsync(ct);
    }

    public async Task MarkPublishedAsync(
        OutboxMessage message,
        CancellationToken ct = default)
    {
        message.Status = OutboxMessageStatuses.Published;
        message.PublishedAtUtc = DateTime.UtcNow;
        message.NextAttemptAtUtc = null;
        message.ErrorMessage = null;

        await _outboxRepository.SaveChangesAsync(ct);
    }

    public async Task MarkFailedAsync(
        OutboxMessage message,
        Exception exception,
        CancellationToken ct = default)
    {
        message.RetryCount++;
        message.Status = OutboxMessageStatuses.Failed;
        message.NextAttemptAtUtc = CalculateNextAttemptUtc(
            message.RetryCount);
        message.ErrorMessage = TruncateErrorMessage(
            exception.Message);

        await _outboxRepository.SaveChangesAsync(ct);
    }

    private static DateTime CalculateNextAttemptUtc(int retryCount)
    {
        var delay = retryCount switch
        {
            1 => TimeSpan.FromSeconds(10),
            2 => TimeSpan.FromSeconds(30),
            3 => TimeSpan.FromMinutes(1),
            4 => TimeSpan.FromMinutes(5),
            _ => TimeSpan.FromMinutes(15)
        };

        return DateTime.UtcNow.Add(delay);
    }

    private static string TruncateErrorMessage(string errorMessage)
    {
        return errorMessage.Length <= MaximumErrorMessageLength
            ? errorMessage
            : errorMessage[..MaximumErrorMessageLength];
    }
}