using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.BackgroundServices;

public sealed class NotificationCleanupService(
    IServiceScopeFactory scopeFactory,
    ILogger<NotificationCleanupService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NotificationCleanupService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanupOldNotificationsAsync(stoppingToken);

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("NotificationCleanupService cancellation requested.");
        }

        logger.LogInformation("NotificationCleanupService stopped.");
    }

    private async Task CleanupOldNotificationsAsync(CancellationToken stoppingToken)
    {
        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<HealthAxisDbContext>();

        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        var oldNotifications = await dbContext.Notifications
            .Where(notification => notification.CreatedAt < cutoffDate)
            .ToListAsync(stoppingToken);

        if (oldNotifications.Count == 0)
        {
            logger.LogInformation("No old notifications found for cleanup.");
            return;
        }

        dbContext.Notifications.RemoveRange(oldNotifications);

        await dbContext.SaveChangesAsync(stoppingToken);

        logger.LogInformation(
            "Deleted {Count} old notification(s) created before {CutoffDate}.",
            oldNotifications.Count,
            cutoffDate);
    }
}