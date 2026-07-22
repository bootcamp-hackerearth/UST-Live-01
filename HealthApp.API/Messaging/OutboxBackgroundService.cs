using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.Shared.Constants;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HealthApp.API.Messaging;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<OutboxBackgroundService> _logger;

    public OutboxBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Outbox background service started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessOutboxMessagesAsync(
                        stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Unexpected error occurred while processing " +
                        "outbox messages.");
                }

                await Task.Delay(
                    TimeSpan.FromSeconds(
                        OutboxConstants.ProcessingIntervalSeconds),
                    stoppingToken);
            }
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Normal application shutdown.
        }
        finally
        {
            _logger.LogInformation(
                "Outbox background service stopped.");
        }
    }

    private async Task ProcessOutboxMessagesAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext =
            scope.ServiceProvider
                .GetRequiredService<HealthAppDbContext>();

        var publishEndpoint =
            scope.ServiceProvider
                .GetRequiredService<IPublishEndpoint>();

        /*
         * Retry every unprocessed message.
         *
         * RetryCount is retained for logging and monitoring,
         * but it does not permanently block publishing when
         * RabbitMQ remains unavailable for a long time.
         */
        var pendingMessages =
            await dbContext.OutboxMessages
                .Where(outboxMessage =>
                    outboxMessage.ProcessedDate == null)
                .OrderBy(outboxMessage =>
                    outboxMessage.CreatedDate)
                .Take(OutboxConstants.BatchSize)
                .ToListAsync(cancellationToken);

        if (pendingMessages.Count == 0)
        {
            return;
        }

        _logger.LogInformation(
            "Processing {OutboxMessageCount} pending " +
            "outbox message(s).",
            pendingMessages.Count);

        foreach (var outboxMessage in pendingMessages)
        {
            try
            {
                switch (outboxMessage.EventType)
                {
                    case OutboxConstants
                        .AppointmentBookedEventType:
                        {
                            var appointmentBookedEvent =
                                JsonSerializer.Deserialize
                                    <AppointmentBookedEvent>(
                                        outboxMessage.Payload);

                            if (appointmentBookedEvent is null)
                            {
                                throw new InvalidOperationException(
                                    "AppointmentBookedEvent payload " +
                                    "could not be deserialized.");
                            }

                            await publishEndpoint.Publish(
                                appointmentBookedEvent,
                                cancellationToken);

                            break;
                        }

                    default:
                        throw new InvalidOperationException(
                            "Unsupported outbox event type: " +
                            outboxMessage.EventType);
                }

                var processedDate = DateTime.UtcNow;

                outboxMessage.ProcessedDate = processedDate;
                outboxMessage.LastAttemptDate = processedDate;
                outboxMessage.ErrorMessage = null;

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                _logger.LogInformation(
                    "Outbox message published successfully. " +
                    "OutboxMessageId: {OutboxMessageId}, " +
                    "EventType: {EventType}, " +
                    "ProcessedDate: {ProcessedDate}",
                    outboxMessage.OutboxMessageId,
                    outboxMessage.EventType,
                    outboxMessage.ProcessedDate);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                outboxMessage.RetryCount++;
                outboxMessage.LastAttemptDate = DateTime.UtcNow;
                outboxMessage.ErrorMessage =
                    TruncateErrorMessage(exception.Message);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                _logger.LogError(
                    exception,
                    "Failed to publish outbox message. " +
                    "The message remains unprocessed and will be " +
                    "retried. OutboxMessageId: {OutboxMessageId}, " +
                    "EventType: {EventType}, " +
                    "RetryCount: {RetryCount}",
                    outboxMessage.OutboxMessageId,
                    outboxMessage.EventType,
                    outboxMessage.RetryCount);
            }
        }
    }

    private static string TruncateErrorMessage(
        string errorMessage)
    {
        if (errorMessage.Length <=
            OutboxConstants.MaximumErrorMessageLength)
        {
            return errorMessage;
        }

        return errorMessage[
            ..OutboxConstants.MaximumErrorMessageLength];
    }
}