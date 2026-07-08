namespace HealthAxis.API.BackgroundServices;

public sealed class HeartbeatService(ILogger<HeartbeatService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("HeartbeatService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("HealthAxis API heartbeat running at {Time}", DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("HeartbeatService cancellation requested.");
        }

        logger.LogInformation("HeartbeatService stopped.");
    }
}