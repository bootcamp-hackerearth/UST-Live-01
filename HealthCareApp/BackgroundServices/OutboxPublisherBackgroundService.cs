using HealthCareApp.Data;
using HealthCareApp.Messaging.Events;
using HealthCareApp.Models;
using HealthCareApp.Shared.Constants;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HealthCareApp.BackgroundServices
{
    public class OutboxPublisherBackgroundService : BackgroundService
    {
        private const int BatchSize = 10;
        private const int MaxRetryCount = 5;
        private const int MaxErrorMessageLength = 1000;

        private const string AppointmentBookedEventType = "AppointmentBooked";
        private const string EventStagePublished = "PublishedFromOutbox";
        private const string EventStagePublishFailed = "OutboxPublishFailed";

        private static readonly TimeSpan PublishInterval = TimeSpan.FromSeconds(10);

        private static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        private readonly IServiceScopeFactory scopeFactory;

        private readonly ILogger<OutboxPublisherBackgroundService> logger;

        public OutboxPublisherBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<OutboxPublisherBackgroundService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            LogOutboxPublisherStarted();

            using var timer = new PeriodicTimer(PublishInterval);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await PublishPendingOutboxMessagesAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                LogOutboxPublisherStopped();
            }
        }

        private async Task PublishPendingOutboxMessagesAsync(
            CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext =
                scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

            var publishEndpoint =
                scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var pendingMessages = await dbContext.OutboxMessages
                .Where(outboxMessage =>
                    (
                        outboxMessage.Status == OutboxMessageStatuses.Pending ||
                        outboxMessage.Status == OutboxMessageStatuses.Failed
                    ) &&
                    outboxMessage.RetryCount < MaxRetryCount)
                .OrderBy(outboxMessage => outboxMessage.CreatedDate)
                .Take(BatchSize)
                .ToListAsync(cancellationToken);

            if (pendingMessages.Count == 0)
            {
                return;
            }

            foreach (var outboxMessage in pendingMessages)
            {
                try
                {
                    await PublishOutboxMessageAsync(
                        publishEndpoint,
                        outboxMessage.EventType,
                        outboxMessage.Payload,
                        cancellationToken);

                    outboxMessage.Status = OutboxMessageStatuses.Published;
                    outboxMessage.PublishedDate = DateTime.Now;
                    outboxMessage.ErrorMessage = null;

                    LogOutboxMessagePublished(outboxMessage);
                }
                catch (Exception ex)
                {
                    outboxMessage.Status = OutboxMessageStatuses.Failed;
                    outboxMessage.RetryCount++;
                    outboxMessage.ErrorMessage =
                        TruncateErrorMessage(ex.Message);

                    LogOutboxMessagePublishFailed(
                        ex,
                        outboxMessage);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task PublishOutboxMessageAsync(
            IPublishEndpoint publishEndpoint,
            string eventType,
            string payload,
            CancellationToken cancellationToken)
        {
            if (eventType == nameof(AppointmentBookedEvent))
            {
                var appointmentBookedEvent =
                    JsonSerializer.Deserialize<AppointmentBookedEvent>(
                        payload,
                        SerializerOptions);

                if (appointmentBookedEvent is null)
                {
                    throw new InvalidOperationException(
                        "AppointmentBookedEvent payload could not be deserialized.");
                }

                await publishEndpoint.Publish(
                    appointmentBookedEvent,
                    cancellationToken);

                return;
            }

            throw new InvalidOperationException(
                $"Unsupported outbox event type: {eventType}");
        }

        private static string TruncateErrorMessage(string errorMessage)
        {
            if (errorMessage.Length <= MaxErrorMessageLength)
            {
                return errorMessage;
            }

            return errorMessage[..MaxErrorMessageLength];
        }

        private void LogOutboxPublisherStarted()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "HealthAxis Outbox Publisher Background Service started.");
        }

        private void LogOutboxPublisherStopped()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "HealthAxis Outbox Publisher Background Service stopped.");
        }

        private void LogOutboxMessagePublished(
            OutboxMessage outboxMessage)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            using var outboxLogScope = BeginOutboxLogScope(
                outboxMessage,
                EventStagePublished);

            logger.LogInformation(
                "Outbox message published successfully to RabbitMQ. EventStage: {EventStage}, OutboxMessageId: {OutboxMessageId}, EventType: {EventType}",
                EventStagePublished,
                outboxMessage.OutboxMessageId,
                outboxMessage.EventType);
        }

        private void LogOutboxMessagePublishFailed(
            Exception exception,
            OutboxMessage outboxMessage)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            using var outboxLogScope = BeginOutboxLogScope(
                outboxMessage,
                EventStagePublishFailed);

            logger.LogWarning(
                exception,
                "Outbox message publish failed. EventStage: {EventStage}, OutboxMessageId: {OutboxMessageId}, EventType: {EventType}, RetryCount: {RetryCount}",
                EventStagePublishFailed,
                outboxMessage.OutboxMessageId,
                outboxMessage.EventType,
                outboxMessage.RetryCount);
        }

        private IDisposable? BeginOutboxLogScope(
            OutboxMessage outboxMessage,
            string eventStage)
        {
            return logger.BeginScope(new Dictionary<string, object>
            {
                ["EventType"] = AppointmentBookedEventType,
                ["EventStage"] = eventStage,
                ["OutboxMessageId"] = outboxMessage.OutboxMessageId,
                ["OutboxEventType"] = outboxMessage.EventType,
                ["OutboxStatus"] = outboxMessage.Status,
                ["OutboxRetryCount"] = outboxMessage.RetryCount
            });
        }
    }
}