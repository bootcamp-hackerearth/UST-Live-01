using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthApp.API.BackgroundServices;

public sealed class HeartbeatService : BackgroundService
{
    private static readonly TimeSpan HeartbeatInterval =
        TimeSpan.FromSeconds(10);

    private readonly ILogger<HeartbeatService> logger;

    public HeartbeatService(
        ILogger<HeartbeatService> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "HeartbeatService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                LogHeartbeat();

                await Task.Delay(
                    HeartbeatInterval,
                    stoppingToken);
            }
        }
        catch (OperationCanceledException exception)
            when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation(
                exception,
                "HeartbeatService cancellation requested.");
        }
        finally
        {
            logger.LogInformation(
                "HeartbeatService stopped.");
        }
    }

    private void LogHeartbeat()
    {
        if (!logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var currentTime = DateTimeOffset.Now;

        logger.LogInformation(
            "HeartbeatService running at: {Time}",
            currentTime);
    }
}