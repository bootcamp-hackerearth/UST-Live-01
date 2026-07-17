using HealthCareApp.Data;
using HealthCareApp.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HealthCareApp.BackgroundServices
{
    public class NotificationCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        private readonly ILogger<NotificationCleanupBackgroundService> logger;

        private readonly NotificationCleanupOptions options;

        public NotificationCleanupBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationCleanupBackgroundService> logger,
            IOptions<NotificationCleanupOptions> options)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
            this.options = options.Value;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "Notification Cleanup Background Service started.");
            }

            using var timer = new PeriodicTimer(
                TimeSpan.FromHours(options.IntervalHours));

            await CleanupNotificationsAsync(stoppingToken);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CleanupNotificationsAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Notification Cleanup Background Service cancellation requested.");
                }
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation(
                    "Notification Cleanup Background Service stopped.");
            }
        }

        private async Task CleanupNotificationsAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();

                var context =
                    scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

                var cutoffDate = DateTime.UtcNow.AddDays(-options.RetentionDays);

                var oldReadNotifications = await context.Notifications
                    .Where(notification =>
                        notification.IsRead &&
                        notification.CreatedDate < cutoffDate)
                    .ToListAsync(cancellationToken);

                if (oldReadNotifications.Count == 0)
                {
                    LogNoNotificationsFound(cutoffDate);

                    return;
                }

                context.Notifications.RemoveRange(oldReadNotifications);

                await context.SaveChangesAsync(cancellationToken);

                LogCleanupSuccess(
                    oldReadNotifications.Count,
                    cutoffDate);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogCleanupFailure(ex);
            }
        }

        private void LogNoNotificationsFound(DateTime cutoffDate)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "No old read RabbitMQ notifications found before {CutoffDate}.",
                cutoffDate);
        }

        private void LogCleanupSuccess(
            int deletedCount,
            DateTime cutoffDate)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Deleted {DeletedCount} old read RabbitMQ notification(s) before {CutoffDate}.",
                deletedCount,
                cutoffDate);
        }

        private void LogCleanupFailure(Exception exception)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Failed to clean up old RabbitMQ notifications.");
        }
    }
}