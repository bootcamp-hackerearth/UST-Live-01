using HealthCareApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.BackgroundServices
{
    public class NotificationCleanupService : BackgroundService
    {
        private const string DateTimeFormat = "dd MMM yyyy hh:mm:ss tt";
        private const string CleanupStatusCompleted = "Completed";
        private const string CleanupRuleDescription = "Delete notifications older than 30 days";
        private const string ShutdownReason = "Application shutdown requested";

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
            LogCleanupServiceStarted();

            try
            {
                await CleanOldNotificationsAsync(stoppingToken);

                using var timer = new PeriodicTimer(CleanupInterval);

                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await CleanOldNotificationsAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException ex)
                when (stoppingToken.IsCancellationRequested)
            {
                LogCleanupServiceStopping(ex);
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
                LogNotificationCleanupCompleted(
                    deletedNotificationCount: 0,
                    cutoffDate);

                return;
            }

            dbContext.Notifications.RemoveRange(oldNotifications);

            await dbContext.SaveChangesAsync(cancellationToken);

            LogNotificationCleanupCompleted(
                oldNotifications.Count,
                cutoffDate);
        }

        private void LogCleanupServiceStarted()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                """
                ============================================================
                | HealthAxis Notification Cleanup Service STARTED          |
                | Purpose : Deletes notifications older than 30 days        |
                | Runs    : Once immediately, then every 1 hour             |
                ============================================================
                """);
        }

        private void LogCleanupServiceStopping(
            OperationCanceledException exception)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                exception,
                """
                ============================================================
                | HealthAxis Notification Cleanup Service STOPPING         |
                | Reason : {ShutdownReason}                               |
                ============================================================
                """,
                ShutdownReason);
        }

        private void LogNotificationCleanupCompleted(
            int deletedNotificationCount,
            DateTime cutoffDate)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                """
                ------------------------------------------------------------
                | HEALTHAXIS NOTIFICATION CLEANUP                         |
                | Status       : {CleanupStatus}                           |
                | Deleted      : {DeletedNotificationCount} notification(s) |
                | Cutoff Date  : {CutoffDate}                              |
                | Rule         : {CleanupRule}                             |
                ------------------------------------------------------------
                """,
                CleanupStatusCompleted,
                deletedNotificationCount,
                FormatDateTime(cutoffDate),
                CleanupRuleDescription);
        }

        private static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString(DateTimeFormat);
        }
    }
}