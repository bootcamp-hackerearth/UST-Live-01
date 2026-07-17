using HealthApp.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.HostedServices;

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
            LogServiceStarted();

            while (!stoppingToken.IsCancellationRequested)
            {
                await DeleteOldNotificationsAsync(stoppingToken);
                await Task.Delay(CleanupInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Expected during application shutdown.
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
            LogServiceStopped();
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
                LogNoExpiredNotifications(cutoffDateUtc);
                return;
            }

            dbContext.Notifications.RemoveRange(oldNotifications);
            await dbContext.SaveChangesAsync(stoppingToken);

            LogExpiredNotificationsDeleted(
                oldNotifications.Count,
                cutoffDateUtc,
                cleanupStartedAtUtc);
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                "Notification cleanup failed for notifications created " +
                $"before {cutoffDateUtc:O}.",
                exception);
        }
    }

    private void LogServiceStarted()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var cleanupIntervalHours = CleanupInterval.TotalHours;

        _logger.LogInformation(
            "Notification cleanup service started with a cleanup interval " +
            "of {CleanupIntervalHours} hour(s) and a retention period of " +
            "{RetentionDays} days. Event type: {EventType}",
            cleanupIntervalHours,
            RetentionDays,
            "NotificationCleanupServiceStarted");
    }

    private void LogServiceStopped()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Notification cleanup service stopped. " +
            "Event type: {EventType}",
            "NotificationCleanupServiceStopped");
    }

    private void LogNoExpiredNotifications(DateTime cutoffDateUtc)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Notification cleanup completed with no expired notifications " +
            "found before {CutoffDateUtc}. Event type: {EventType}",
            cutoffDateUtc,
            "NotificationCleanupCompleted");
    }

    private void LogExpiredNotificationsDeleted(
        int deletedNotificationCount,
        DateTime cutoffDateUtc,
        DateTime cleanupStartedAtUtc)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var elapsedMilliseconds =
            (DateTime.UtcNow - cleanupStartedAtUtc).TotalMilliseconds;

        _logger.LogInformation(
            "Notification cleanup deleted {DeletedNotificationCount} " +
            "notification(s) created before {CutoffDateUtc} in " +
            "{ElapsedMilliseconds} ms. Event type: {EventType}",
            deletedNotificationCount,
            cutoffDateUtc,
            elapsedMilliseconds,
            "ExpiredNotificationsDeleted");
    }
}