using HealthApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Services.Impl
{
    public class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan InitialDelay =
            TimeSpan.FromSeconds(3);

        private static readonly TimeSpan CleanupInterval =
            TimeSpan.FromHours(1);

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
                await Task.Delay(InitialDelay, stoppingToken);

                _logger.LogInformation(
                    "Notification cleanup service started with a cleanup interval of {CleanupIntervalHours} hour(s) and a retention period of {RetentionDays} days. {EventType}",
                    CleanupInterval.TotalHours,
                    RetentionDays,
                    "NotificationCleanupServiceStarted");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await DeleteOldNotificationsAsync(stoppingToken);

                    await Task.Delay(
                        CleanupInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Notification cleanup service received a shutdown signal. {EventType}",
                    "NotificationCleanupServiceStopping");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Notification cleanup service stopped because of an unexpected error. {EventType}",
                    "NotificationCleanupServiceFailed");

                throw;
            }
            finally
            {
                _logger.LogInformation(
                    "Notification cleanup service stopped. {EventType}",
                    "NotificationCleanupServiceStopped");
            }
        }

        private async Task DeleteOldNotificationsAsync(
            CancellationToken stoppingToken)
        {
            var cleanupStartedAtUtc = DateTime.UtcNow;
            var cutoffDateUtc = cleanupStartedAtUtc.AddDays(-RetentionDays);

            try
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider
                    .GetRequiredService<HealthAppDbContext>();

                var oldNotifications = await dbContext.Notifications
                    .Where(notification =>
                        notification.CreatedAt < cutoffDateUtc)
                    .ToListAsync(stoppingToken);

                if (oldNotifications.Count == 0)
                {
                    _logger.LogDebug(
                        "Notification cleanup completed with no expired notifications found before {CutoffDateUtc}. {EventType}",
                        cutoffDateUtc,
                        "NotificationCleanupCompleted");

                    return;
                }

                dbContext.Notifications.RemoveRange(oldNotifications);

                await dbContext.SaveChangesAsync(stoppingToken);

                var elapsedMilliseconds =
                    (DateTime.UtcNow - cleanupStartedAtUtc).TotalMilliseconds;

                _logger.LogInformation(
                    "Notification cleanup deleted {DeletedNotificationCount} notification(s) created before {CutoffDateUtc} in {ElapsedMilliseconds} ms. {EventType}",
                    oldNotifications.Count,
                    cutoffDateUtc,
                    elapsedMilliseconds,
                    "ExpiredNotificationsDeleted");
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Notification cleanup failed for notifications created before {CutoffDateUtc}. {EventType}",
                    cutoffDateUtc,
                    "NotificationCleanupFailed");

                throw;
            }
        }
    }
}
