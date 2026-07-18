using HealthApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthApp.API.BackgroundServices;

public sealed class NotificationCleanupService : BackgroundService
{
    private static readonly TimeSpan CleanupInterval =
        TimeSpan.FromHours(1);

    private const int NotificationRetentionDays = 30;

    private readonly IServiceScopeFactory scopeFactory;

    private readonly ILogger<NotificationCleanupService> logger;

    public NotificationCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationCleanupService> logger)
    {
        this.scopeFactory = scopeFactory;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "NotificationCleanupService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupOldNotificationsAsync(
                    stoppingToken);

                await Task.Delay(
                    CleanupInterval,
                    stoppingToken);
            }
        }
        catch (OperationCanceledException exception)
            when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation(
                exception,
                "NotificationCleanupService shutdown requested.");
        }
        finally
        {
            logger.LogInformation(
                "NotificationCleanupService stopped.");
        }
    }

    private async Task CleanupOldNotificationsAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            using var scope =
                scopeFactory.CreateScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<HealthAppDbContext>();

            var cutoffDate =
                DateTime.Now.AddDays(
                    -NotificationRetentionDays);

            var deletedCount =
                await dbContext.Notifications
                    .Where(notification =>
                        notification.CreatedDate <
                        cutoffDate)
                    .ExecuteDeleteAsync(
                        stoppingToken);

            LogCleanupResult(
                deletedCount,
                cutoffDate);
        }
        catch (OperationCanceledException exception)
            when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation(
                exception,
                "Notification cleanup cancelled during shutdown.");
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "An error occurred while cleaning up old notifications.");
        }
    }

    private void LogCleanupResult(
        int deletedCount,
        DateTime cutoffDate)
    {
        if (!logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        logger.LogInformation(
            "Notification cleanup completed. Deleted {DeletedCount} notifications older than {CutoffDate}.",
            deletedCount,
            cutoffDate);
    }
}