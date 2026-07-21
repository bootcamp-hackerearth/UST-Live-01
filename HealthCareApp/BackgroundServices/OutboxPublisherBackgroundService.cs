using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HealthCareApp.BackgroundServices
{
    public class OutboxPublisherBackgroundService : BackgroundService
    {
        private const int MaxRetryCount = 3;

        private static readonly TimeSpan PublishInterval =
            TimeSpan.FromSeconds(10);

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
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "Outbox Publisher Background Service started.");
            }

            using var timer = new PeriodicTimer(PublishInterval);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await PublishPendingMessagesAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Outbox Publisher Background Service cancellation requested.");
                }
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "Outbox Publisher Background Service stopped.");
            }
        }

        private async Task PublishPendingMessagesAsync(
            CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();

            var context =
                scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

            var publishEndpoint =
                scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var outboxMessages = await context.OutboxMessages
                .Where(message =>
                    (
                        message.Status == OutboxMessageStatuses.Pending ||
                        message.Status == OutboxMessageStatuses.Failed
                    ) &&
                    message.RetryCount < MaxRetryCount)
                .OrderBy(message => message.CreatedDateUtc)
                .Take(20)
                .ToListAsync(cancellationToken);

            if (outboxMessages.Count == 0)
            {
                return;
            }

            foreach (var outboxMessage in outboxMessages)
            {
                await PublishMessageAsync(
                    outboxMessage,
                    publishEndpoint,
                    context,
                    cancellationToken);
            }
        }

        private async Task PublishMessageAsync(
            OutboxMessage outboxMessage,
            IPublishEndpoint publishEndpoint,
            HealthAxisDbContext context,
            CancellationToken cancellationToken)
        {
            try
            {
                if (outboxMessage.EventType == nameof(AppointmentBookedEvent))
                {
                    var appointmentBookedEvent =
                        JsonSerializer.Deserialize<AppointmentBookedEvent>(
                            outboxMessage.Payload);

                    if (appointmentBookedEvent is null)
                    {
                        throw new InvalidOperationException(
                            "Unable to deserialize AppointmentBookedEvent payload.");
                    }

                    await publishEndpoint.Publish(
                        appointmentBookedEvent,
                        cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Unsupported outbox event type: {outboxMessage.EventType}");
                }

                outboxMessage.Status = OutboxMessageStatuses.Published;
                outboxMessage.PublishedDateUtc = DateTime.UtcNow;
                outboxMessage.ErrorMessage = null;

                await context.SaveChangesAsync(cancellationToken);

                LogPublished(outboxMessage);
            }
            catch (Exception ex)
            {
                outboxMessage.RetryCount++;

                outboxMessage.Status =
                    outboxMessage.RetryCount >= MaxRetryCount
                        ? OutboxMessageStatuses.Failed
                        : OutboxMessageStatuses.Pending;

                outboxMessage.ErrorMessage = ex.Message;

                await context.SaveChangesAsync(cancellationToken);

                LogPublishFailure(ex, outboxMessage);
            }
        }

        private void LogPublished(OutboxMessage outboxMessage)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Outbox message {OutboxMessageId} published successfully.",
                outboxMessage.OutboxMessageId);
        }

        private void LogPublishFailure(
            Exception exception,
            OutboxMessage outboxMessage)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Failed to publish outbox message {OutboxMessageId}. RetryCount={RetryCount}",
                outboxMessage.OutboxMessageId,
                outboxMessage.RetryCount);
        }
    }
}