using HealthApp.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.BackgroundServices;

public class NotificationCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationCleanupService> _logger;

    public NotificationCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationCleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationCleanupService started.");

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
            _logger.LogInformation("NotificationCleanupService shutdown requested.");
        }

        _logger.LogInformation("NotificationCleanupService stopped.");
    }

    private async Task CleanupOldNotificationsAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<HealthAppDbContext>();

            var cutoffDate = DateTime.Now.AddDays(-30);

            var deletedCount = await dbContext.Notifications
                .Where(notification => notification.CreatedDate < cutoffDate)
                .ExecuteDeleteAsync(stoppingToken);

            _logger.LogInformation(
                "Notification cleanup completed. Deleted {DeletedCount} notifications older than {CutoffDate}.",
                deletedCount,
                cutoffDate);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Notification cleanup cancelled during shutdown.");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while cleaning up old notifications.");
        }
    }
}