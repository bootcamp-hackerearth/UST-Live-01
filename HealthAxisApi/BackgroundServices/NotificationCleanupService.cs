using HealthAxisCore_Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class NotificationCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NotificationCleanupService> _logger;

        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(6);

        private const int RetentionDays = 30;

        public NotificationCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "NotificationCleanupService started at {Time}",
                DateTimeOffset.Now
            );

            using var timer = new PeriodicTimer(CleanupInterval);

            try
            {
                await CleanupOldNotificationsAsync(stoppingToken);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CleanupOldNotificationsAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "NotificationCleanupService cancellation requested."
                );
            }
            finally
            {
                _logger.LogInformation(
                    "NotificationCleanupService stopped at {Time}",
                    DateTimeOffset.Now
                );
            }
        }

        private async Task CleanupOldNotificationsAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext =
                    scope.ServiceProvider.GetRequiredService<HealthAppDbContext>();

                var cutoffDate = DateTime.Now.AddDays(-RetentionDays);

                var oldReadNotifications = await dbContext.Notifications
                    .Where(notification =>
                        notification.IsRead &&
                        notification.CreatedDate < cutoffDate)
                    .ToListAsync(cancellationToken);

                if (!oldReadNotifications.Any())
                {
                    _logger.LogInformation(
                        "Notification cleanup completed at {Time}. No read notifications older than {RetentionDays} days were found.",
                        DateTimeOffset.Now,
                        RetentionDays
                    );

                    return;
                }

                dbContext.Notifications.RemoveRange(oldReadNotifications);

                await dbContext.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Notification cleanup completed at {Time}. Deleted {Count} read notifications older than {RetentionDays} days.",
                    DateTimeOffset.Now,
                    oldReadNotifications.Count,
                    RetentionDays
                );
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while cleaning old notifications at {Time}",
                    DateTimeOffset.Now
                );
            }
        }

        public override async Task StopAsync(
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "NotificationCleanupService is stopping gracefully at {Time}",
                DateTimeOffset.Now
            );

            await base.StopAsync(cancellationToken);
        }
    }
}