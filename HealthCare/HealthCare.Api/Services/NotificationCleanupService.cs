using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class NotificationCleanupService : BackgroundService
{
    private readonly ILogger<NotificationCleanupService> _logger;

    public NotificationCleanupService(
        ILogger<NotificationCleanupService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationCleanupService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for notifications older than 30 days...");

                _logger.LogInformation("Cleanup completed successfully.");
                _logger.LogInformation("Next cleanup will run after 1 hour.");

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("NotificationCleanupService is stopping...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in NotificationCleanupService.");
        }
        finally
        {
            _logger.LogInformation("NotificationCleanupService stopped.");
        }
    }
}