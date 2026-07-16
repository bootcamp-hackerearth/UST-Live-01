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

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var cleanupIntervalHours = CleanupInterval.TotalHours;

                    _logger.LogInformation(
                        "Notification cleanup service started with a cleanup interval of {CleanupIntervalHours} hour(s) and a retention period of {RetentionDays} days. {EventType}",
                        cleanupIntervalHours,
                        RetentionDays,
                        "NotificationCleanupServiceStarted");
                }

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
                // Expected during application shutdown. The final log entry
                // records that the cleanup service stopped normally.
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    "Notification cleanup service stopped because of an " +
                    "unexpected error.",
                    exception);
            }
            finally
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Notification cleanup service stopped. {EventType}",
                        "NotificationCleanupServiceStopped");
                }
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
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug(
                            "Notification cleanup completed with no expired notifications found before {CutoffDateUtc}. {EventType}",
                            cutoffDateUtc,
                            "NotificationCleanupCompleted");
                    }

                    return;
                }

                dbContext.Notifications.RemoveRange(oldNotifications);

                await dbContext.SaveChangesAsync(stoppingToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var elapsedMilliseconds =
                        (DateTime.UtcNow - cleanupStartedAtUtc)
                        .TotalMilliseconds;

                    _logger.LogInformation(
                        "Notification cleanup deleted {DeletedNotificationCount} notification(s) created before {CutoffDateUtc} in {ElapsedMilliseconds} ms. {EventType}",
                        oldNotifications.Count,
                        cutoffDateUtc,
                        elapsedMilliseconds,
                        "ExpiredNotificationsDeleted");
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Notification cleanup failed for notifications created " +
                    $"before {cutoffDateUtc:O}.",
                    exception);
            }
        }
    }
}
