using HealthApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Services.Impl
{
    public class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan InitialDelay =
            TimeSpan.FromSeconds(3);

        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(1);

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
            try
            {
                await Task.Delay(
                    InitialDelay,
                    stoppingToken);

                _logger.LogInformation(
                    "\n" +
                    "==================================================\n" +
                    " NOTIFICATION CLEANUP SERVICE STARTED\n" +
                    " Interval       : {IntervalMinutes} minute(s)\n" +
                    " Retention Days : {RetentionDays} days\n" +
                    " Started At UTC : {StartedAtUtc}\n" +
                    "==================================================",
                    CleanupInterval.TotalMinutes,
                    RetentionDays,
                    DateTime.UtcNow);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await DeleteOldNotificationsAsync(stoppingToken);

                    await Task.Delay(
                        CleanupInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "\n" +
                    "---------------------------------------------------\n" +
                    " NOTIFICATION CLEANUP SHUTDOWN SIGNAL RECEIVED\n" +
                    "---------------------------------------------------");
            }
            finally
            {
                _logger.LogInformation(
                    "\n" +
                    "==================================================\n" +
                    " NOTIFICATION CLEANUP SERVICE STOPPED GRACEFULLY\n" +
                    " Stopped At UTC : {StoppedAtUtc}\n" +
                    "==================================================",
                    DateTime.UtcNow);
            }
        }

        private async Task DeleteOldNotificationsAsync(
            CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<HealthAppDbContext>();

            var cutoffDate = DateTime.UtcNow.AddDays(-RetentionDays);

            var oldNotifications = await dbContext.Notifications
                .Where(notification => notification.CreatedAt < cutoffDate)
                .ToListAsync(stoppingToken);

            if (oldNotifications.Count == 0)
            {
                _logger.LogInformation(
                    "\n" +
                    "---------------- NOTIFICATION CLEANUP ----------------\n" +
                    " Status         : No old notifications found\n" +
                    " Cutoff Date UTC: {CutoffDateUtc}\n" +
                    " Checked At UTC : {CheckedAtUtc}\n" +
                    "------------------------------------------------------",
                    cutoffDate,
                    DateTime.UtcNow);

                return;
            }

            dbContext.Notifications.RemoveRange(oldNotifications);

            await dbContext.SaveChangesAsync(stoppingToken);

            _logger.LogInformation(
                "\n" +
                "---------------- NOTIFICATION CLEANUP ----------------\n" +
                " Status         : Old notifications deleted\n" +
                " Deleted Count  : {DeletedCount}\n" +
                " Cutoff Date UTC: {CutoffDateUtc}\n" +
                " Cleaned At UTC : {CleanedAtUtc}\n" +
                "------------------------------------------------------",
                oldNotifications.Count,
                cutoffDate,
                DateTime.UtcNow);
        }
    }
}