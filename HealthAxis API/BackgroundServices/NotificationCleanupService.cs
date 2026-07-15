using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.BackgroundServices
{
    public class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan CleanupInterval =
            TimeSpan.FromHours(1);

        private const int NotificationRetentionDays = 30;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationCleanupService> _logger;

        public NotificationCleanupService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Notification cleanup service started at {StartedAt}",
                    DateTimeOffset.Now);
            }

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await CleanupOldNotificationsAsync(stoppingToken);

                    await Task.Delay(
                        CleanupInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                // Expected when the application is shutting down.
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Notification cleanup service stopped because of an unexpected error.");
            }
            finally
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Notification cleanup service stopped at {StoppedAt}",
                        DateTimeOffset.Now);
                }
            }
        }

        private async Task CleanupOldNotificationsAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using IServiceScope scope =
                    _serviceScopeFactory.CreateScope();

                HealthAxisDbContext dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<HealthAxisDbContext>();

                DateTime cutoffDate =
                    DateTime.UtcNow.AddDays(-NotificationRetentionDays);

                List<Models.Notification> oldNotifications =
                    await dbContext.Notifications
                        .Where(notification =>
                            notification.CreatedAt < cutoffDate)
                        .ToListAsync(cancellationToken);

                if (oldNotifications.Count == 0)
                {
                    _logger.LogInformation(
                        "Notification cleanup completed. No old notifications found.");

                    return;
                }

                dbContext.Notifications.RemoveRange(
                    oldNotifications);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Notification cleanup completed. Deleted {DeletedCount} notifications older than {RetentionDays} days.",
                        oldNotifications.Count,
                        NotificationRetentionDays);
                }
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                // Expected during application shutdown.
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "An error occurred while cleaning old notifications.");
            }
        }
    }
}
