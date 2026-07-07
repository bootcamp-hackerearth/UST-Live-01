using HealthCareApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.BackgroundServices
{
    public class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(1);

        private static readonly TimeSpan NotificationRetentionPeriod = TimeSpan.FromDays(30);

        private readonly IServiceScopeFactory scopeFactory;

        private readonly ILogger<NotificationCleanupService> logger;

        public NotificationCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                """
                ============================================================
                | HealthAxis Notification Cleanup Service STARTED          |
                | Purpose : Deletes notifications older than 30 days        |
                | Runs    : Once immediately, then every 1 hour             |
                ============================================================
                """);

            try
            {
                await CleanOldNotificationsAsync(stoppingToken);

                using var timer = new PeriodicTimer(CleanupInterval);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CleanOldNotificationsAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation(
                    """
                    ============================================================
                    | HealthAxis Notification Cleanup Service STOPPING         |
                    | Reason : Application shutdown requested                  |
                    ============================================================
                    """);
            }
        }

        private async Task CleanOldNotificationsAsync(CancellationToken cancellationToken)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

            var cutoffDate = DateTime.Now.Subtract(NotificationRetentionPeriod);

            var oldNotifications = await dbContext.Notifications
                .Where(notification => notification.CreatedDate < cutoffDate)
                .ToListAsync(cancellationToken);

            if (oldNotifications.Count == 0)
            {
                logger.LogInformation(
                    """
                    ------------------------------------------------------------
                    | HEALTHAXIS NOTIFICATION CLEANUP                         |
                    | Status       : Completed                                 |
                    | Deleted      : 0 notification(s)                         |
                    | Cutoff Date  : {CutoffDate}                              |
                    | Rule         : Delete notifications older than 30 days    |
                    ------------------------------------------------------------
                    """,
                    cutoffDate.ToString("dd MMM yyyy hh:mm:ss tt"));

                return;
            }

            dbContext.Notifications.RemoveRange(oldNotifications);

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                """
                ------------------------------------------------------------
                | HEALTHAXIS NOTIFICATION CLEANUP                         |
                | Status       : Completed                                 |
                | Deleted      : {Count} notification(s)                    |
                | Cutoff Date  : {CutoffDate}                              |
                | Rule         : Delete notifications older than 30 days    |
                ------------------------------------------------------------
                """,
                oldNotifications.Count,
                cutoffDate.ToString("dd MMM yyyy hh:mm:ss tt"));
        }
    }
}