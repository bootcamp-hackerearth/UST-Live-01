using System.Text.Json;

using HealthAxisCore_Api.Constants;
using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.BackgroundServices
{
    public sealed class OutboxPublisherBackgroundService
        : BackgroundService
    {
        private const int BatchSize = 20;

        private const int MaximumRetryCount = 10;

        private const int MaximumErrorMessageLength = 2000;

        private static readonly TimeSpan PollingInterval =
            TimeSpan.FromSeconds(10);

        private static readonly TimeSpan RetryDelay =
            TimeSpan.FromSeconds(30);

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<
            OutboxPublisherBackgroundService> _logger;

        public OutboxPublisherBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<
                OutboxPublisherBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            LogServiceStarted();

            using var timer =
                new PeriodicTimer(PollingInterval);

            try
            {
                /*
                 * Process the outbox immediately when the API starts.
                 * The service does not have to wait for the first
                 * 10-second timer interval.
                 */
                await ProcessOutboxMessagesAsync(
                    stoppingToken);

                while (await timer.WaitForNextTickAsync(
                    stoppingToken))
                {
                    await ProcessOutboxMessagesAsync(
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                LogCancellationRequested();
            }
            finally
            {
                LogServiceStopped();
            }
        }

        private async Task ProcessOutboxMessagesAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<
                            HealthAppDbContext>();

                var publishEndpoint =
                    scope.ServiceProvider
                        .GetRequiredService<
                            IPublishEndpoint>();

                var currentTime =
                    DateTime.UtcNow;

                var outboxMessages =
                    await dbContext.OutboxMessages
                        .Where(message =>
                            (
                                message.Status ==
                                    OutboxMessageStatuses.Pending ||
                                message.Status ==
                                    OutboxMessageStatuses.Failed
                            ) &&
                            message.RetryCount <
                                MaximumRetryCount &&
                            (
                                message.NextRetryDate == null ||
                                message.NextRetryDate <=
                                    currentTime
                            ))
                        .OrderBy(message =>
                            message.CreatedDate)
                        .Take(BatchSize)
                        .ToListAsync(
                            cancellationToken);

                if (outboxMessages.Count == 0)
                {
                    return;
                }

                LogBatchStarted(
                    outboxMessages.Count);

                foreach (var outboxMessage
                    in outboxMessages)
                {
                    cancellationToken
                        .ThrowIfCancellationRequested();

                    await PublishOutboxMessageAsync(
                        outboxMessage,
                        publishEndpoint,
                        dbContext,
                        cancellationToken);
                }
            }
            catch (OperationCanceledException)
                when (cancellationToken
                    .IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                LogBatchFailure(exception);
            }
        }

        private async Task PublishOutboxMessageAsync(
            OutboxMessage outboxMessage,
            IPublishEndpoint publishEndpoint,
            HealthAppDbContext dbContext,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!IsSupportedEventType(
                    outboxMessage.EventType))
                {
                    throw new InvalidOperationException(
                        $"Unsupported outbox event type: " +
                        $"{outboxMessage.EventType}");
                }

                var appointmentBookedEvent =
                    DeserializeAppointmentBookedEvent(
                        outboxMessage);

                await publishEndpoint.Publish(
                    appointmentBookedEvent,
                    cancellationToken);

                MarkAsPublished(outboxMessage);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                LogMessagePublished(
                    outboxMessage.OutboxMessageId,
                    outboxMessage.EventId);
            }
            catch (OperationCanceledException)
                when (cancellationToken
                    .IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                MarkAsFailed(
                    outboxMessage,
                    exception);

                /*
                 * Save the failed attempt separately so the retry
                 * information remains available after restart.
                 */
                await dbContext.SaveChangesAsync(
                    cancellationToken);

                LogMessagePublishFailed(
                    exception,
                    outboxMessage.OutboxMessageId,
                    outboxMessage.EventId,
                    outboxMessage.RetryCount);
            }
        }

        private static bool IsSupportedEventType(
            string eventType)
        {
            return string.Equals(
                eventType,
                nameof(AppointmentBookedEvent),
                StringComparison.Ordinal);
        }

        private static AppointmentBookedEvent
            DeserializeAppointmentBookedEvent(
                OutboxMessage outboxMessage)
        {
            if (string.IsNullOrWhiteSpace(
                outboxMessage.Payload))
            {
                throw new JsonException(
                    "The outbox payload is empty.");
            }

            var appointmentBookedEvent =
                JsonSerializer.Deserialize<
                    AppointmentBookedEvent>(
                        outboxMessage.Payload);

            if (appointmentBookedEvent == null)
            {
                throw new JsonException(
                    "The AppointmentBookedEvent payload " +
                    "could not be deserialized.");
            }

            return appointmentBookedEvent;
        }

        private static void MarkAsPublished(
            OutboxMessage outboxMessage)
        {
            var currentTime =
                DateTime.UtcNow;

            outboxMessage.Status =
                OutboxMessageStatuses.Published;

            outboxMessage.PublishedDate =
                currentTime;

            outboxMessage.LastAttemptDate =
                currentTime;

            outboxMessage.NextRetryDate =
                null;

            outboxMessage.ErrorMessage =
                null;
        }

        private static void MarkAsFailed(
            OutboxMessage outboxMessage,
            Exception exception)
        {
            var currentTime =
                DateTime.UtcNow;

            outboxMessage.Status =
                OutboxMessageStatuses.Failed;

            outboxMessage.RetryCount++;

            outboxMessage.LastAttemptDate =
                currentTime;

            outboxMessage.NextRetryDate =
                currentTime.Add(RetryDelay);

            outboxMessage.ErrorMessage =
                TruncateErrorMessage(
                    exception.Message);
        }

        private static string TruncateErrorMessage(
            string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(
                errorMessage))
            {
                return "Unknown RabbitMQ publication error.";
            }

            return errorMessage.Length <=
                MaximumErrorMessageLength
                    ? errorMessage
                    : errorMessage[
                        ..MaximumErrorMessageLength];
        }

        private void LogServiceStarted()
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "OutboxPublisherBackgroundService " +
                "started. Polling every " +
                "{PollingIntervalSeconds} seconds.",
                PollingInterval.TotalSeconds);
        }

        private void LogCancellationRequested()
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "OutboxPublisherBackgroundService " +
                "cancellation requested.");
        }

        private void LogServiceStopped()
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "OutboxPublisherBackgroundService stopped.");
        }

        private void LogBatchStarted(
            int messageCount)
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Processing {MessageCount} " +
                "outbox messages.",
                messageCount);
        }

        private void LogMessagePublished(
            long outboxMessageId,
            Guid eventId)
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Outbox message {OutboxMessageId} " +
                "with event {EventId} was published " +
                "to RabbitMQ successfully.",
                outboxMessageId,
                eventId);
        }

        private void LogMessagePublishFailed(
            Exception exception,
            long outboxMessageId,
            Guid eventId,
            int retryCount)
        {
            if (!_logger.IsEnabled(
                LogLevel.Error))
            {
                return;
            }

            _logger.LogError(
                exception,
                "Failed to publish outbox message " +
                "{OutboxMessageId} with event {EventId}. " +
                "Retry count: {RetryCount}.",
                outboxMessageId,
                eventId,
                retryCount);
        }

        private void LogBatchFailure(
            Exception exception)
        {
            if (!_logger.IsEnabled(
                LogLevel.Error))
            {
                return;
            }

            _logger.LogError(
                exception,
                "An unexpected error occurred while " +
                "processing the outbox message batch.");
        }
    }
}