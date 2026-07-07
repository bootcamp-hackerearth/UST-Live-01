using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.BackgroundServices
{
    public class NotificationCleanupService : BackgroundService
    {
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
            _logger.LogInformation(
                "Notification cleanup service started at {Time}",
                DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldNotificationsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "An error occurred while cleaning old notifications.");
                }

                await Task.Delay(
                    TimeSpan.FromHours(1),
                    stoppingToken);
            }

            _logger.LogInformation(
                "Notification cleanup service stopped at {Time}",
                DateTimeOffset.Now);
        }

        private async Task CleanupOldNotificationsAsync(
            CancellationToken cancellationToken)
        {
            using IServiceScope scope =
                _serviceScopeFactory.CreateScope();

            HealthAxisDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

            DateTime cutoffDate =
                DateTime.UtcNow.AddDays(-30);

            var oldNotifications =
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

            dbContext.Notifications.RemoveRange(oldNotifications);

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Notification cleanup completed. Deleted {Count} old notifications.",
                oldNotifications.Count);
        }
    }
}
