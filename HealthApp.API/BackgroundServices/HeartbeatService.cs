using Microsoft.Extensions.Hosting;

namespace HealthApp.API.BackgroundServices;

public class HeartbeatService : BackgroundService
{
    private readonly ILogger<HeartbeatService> _logger;

    public HeartbeatService(ILogger<HeartbeatService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HeartbeatService started.");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "HeartbeatService running at: {Time}",
                    DateTimeOffset.Now);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            
        }

        _logger.LogInformation("HeartbeatService stopped.");
    }
}