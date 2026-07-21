using HealthAxisCore_Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
{
    public sealed class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan CleanupInterval =
            TimeSpan.FromHours(6);

        private const int RetentionDays = 30;

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<NotificationCleanupService> _logger;

        public NotificationCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            LogServiceStarted();

            using var timer =
                new PeriodicTimer(CleanupInterval);

            try
            {
                await CleanupOldNotificationsAsync(
                    stoppingToken);

                while (await timer.WaitForNextTickAsync(
                    stoppingToken))
                {
                    await CleanupOldNotificationsAsync(
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

        private async Task CleanupOldNotificationsAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<HealthAppDbContext>();

                var cutoffDate =
                    DateTime.Now.AddDays(-RetentionDays);

                var oldReadNotifications =
                    await dbContext.Notifications
                        .Where(notification =>
                            notification.IsRead &&
                            notification.CreatedDate < cutoffDate)
                        .ToListAsync(cancellationToken);

                if (oldReadNotifications.Count == 0)
                {
                    LogNoNotificationsFound();
                    return;
                }

                dbContext.Notifications.RemoveRange(
                    oldReadNotifications);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                LogNotificationsDeleted(
                    oldReadNotifications.Count);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                LogCleanupError(exception);
            }
        }

        public override async Task StopAsync(
            CancellationToken cancellationToken)
        {
            LogServiceStopping();

            await base.StopAsync(cancellationToken);
        }

        private void LogServiceStarted()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "NotificationCleanupService started at {Time}",
                DateTimeOffset.Now);
        }

        private void LogCancellationRequested()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "NotificationCleanupService cancellation requested.");
        }

        private void LogServiceStopped()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "NotificationCleanupService stopped at {Time}",
                DateTimeOffset.Now);
        }

        private void LogNoNotificationsFound()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Notification cleanup completed at {Time}. " +
                "No read notifications older than " +
                "{RetentionDays} days were found.",
                DateTimeOffset.Now,
                RetentionDays);
        }

        private void LogNotificationsDeleted(
            int notificationCount)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Notification cleanup completed at {Time}. " +
                "Deleted {Count} read notifications older than " +
                "{RetentionDays} days.",
                DateTimeOffset.Now,
                notificationCount,
                RetentionDays);
        }

        private void LogCleanupError(
            Exception exception)
        {
            if (!_logger.IsEnabled(LogLevel.Error))
            {
                return;
            }

            _logger.LogError(
                exception,
                "Error occurred while cleaning old notifications " +
                "at {Time}",
                DateTimeOffset.Now);
        }

        private void LogServiceStopping()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "NotificationCleanupService is stopping " +
                "gracefully at {Time}",
                DateTimeOffset.Now);
        }
    }
}