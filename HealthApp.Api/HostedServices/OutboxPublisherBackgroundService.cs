using HealthApp.Api.Models;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Events;
using MassTransit;
using System.Text.Json;

namespace HealthApp.Api.HostedServices;

public class OutboxPublisherBackgroundService : BackgroundService
{
    private static readonly TimeSpan InitialDelay =
        TimeSpan.FromSeconds(5);

    private static readonly TimeSpan PollingInterval =
        TimeSpan.FromSeconds(10);

    private static readonly TimeSpan ProcessingTimeout =
        TimeSpan.FromMinutes(1);

    private const int BatchSize = 20;
    private const int MaximumRetryCount = 5;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxPublisherBackgroundService> _logger;

    public OutboxPublisherBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxPublisherBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(InitialDelay, stoppingToken);
            LogServiceStarted();

            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishPendingMessagesAsync(stoppingToken);
                await Task.Delay(PollingInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Expected during application shutdown.
        }
        finally
        {
            LogServiceStopped();
        }
    }

    private async Task PublishPendingMessagesAsync(
        CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var outboxService = scope.ServiceProvider
            .GetRequiredService<IOutboxService>();

        var publishEndpoint = scope.ServiceProvider
            .GetRequiredService<IPublishEndpoint>();

        var messages = await outboxService.GetEligibleMessagesAsync(
            MaximumRetryCount,
            BatchSize,
            ProcessingTimeout,
            stoppingToken);

        if (messages.Count == 0)
        {
            LogNoPendingMessages();
            return;
        }

        foreach (var message in messages)
        {
            await PublishMessageAsync(
                outboxService,
                publishEndpoint,
                message,
                stoppingToken);
        }
    }

    private async Task PublishMessageAsync(
        IOutboxService outboxService,
        IPublishEndpoint publishEndpoint,
        OutboxMessage outboxMessage,
        CancellationToken stoppingToken)
    {
        await outboxService.MarkProcessingAsync(
            outboxMessage,
            stoppingToken);

        try
        {
            await PublishByEventTypeAsync(
                publishEndpoint,
                outboxMessage,
                stoppingToken);

            await outboxService.MarkPublishedAsync(
                outboxMessage,
                stoppingToken);

            LogMessagePublished(outboxMessage);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            await outboxService.MarkFailedAsync(
                outboxMessage,
                exception,
                CancellationToken.None);

            LogMessageFailed(outboxMessage, exception);
        }
    }

    private static async Task PublishByEventTypeAsync(
        IPublishEndpoint publishEndpoint,
        OutboxMessage outboxMessage,
        CancellationToken stoppingToken)
    {
        switch (outboxMessage.EventType)
        {
            case OutboxEventTypes.AppointmentBooked:
                var appointmentBookedEvent =
                    JsonSerializer.Deserialize<AppointmentBookedEvent>(
                        outboxMessage.Payload)
                    ?? throw new InvalidOperationException(
                        "The appointment-booked outbox payload could not be deserialized.");

                await publishEndpoint.Publish(
                    appointmentBookedEvent,
                    stoppingToken);
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported outbox event type: {outboxMessage.EventType}.");
        }
    }

    private void LogServiceStarted()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Outbox publisher service started with a polling interval of {PollingIntervalSeconds} seconds. Event type: {EventType}",
            PollingInterval.TotalSeconds,
            "OutboxPublisherServiceStarted");
    }

    private void LogServiceStopped()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Outbox publisher service stopped. Event type: {EventType}",
            "OutboxPublisherServiceStopped");
    }

    private void LogNoPendingMessages()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Outbox polling completed with no eligible messages. Event type: {EventType}",
            "OutboxPollingCompleted");
    }

    private void LogMessagePublished(OutboxMessage message)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Outbox message {OutboxMessageId} with event {OutboxEventId} was published successfully. Event type: {EventType}",
            message.OutboxMessageId,
            message.EventId,
            "OutboxMessagePublished");
    }

    private void LogMessageFailed(
        OutboxMessage message,
        Exception exception)
    {
        if (!_logger.IsEnabled(LogLevel.Warning))
        {
            return;
        }

        _logger.LogWarning(
            exception,
            "Outbox message {OutboxMessageId} with event {OutboxEventId} failed to publish. Retry {RetryCount} of {MaximumRetryCount}; next attempt at {NextAttemptAtUtc}. Event type: {EventType}",
            message.OutboxMessageId,
            message.EventId,
            message.RetryCount,
            MaximumRetryCount,
            message.NextAttemptAtUtc,
            "OutboxMessagePublishFailed");
    }
}