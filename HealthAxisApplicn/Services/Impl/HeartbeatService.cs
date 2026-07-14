public class HeartbeatService : BackgroundService
{
    private readonly ILogger<HeartbeatService> _logger;

    public HeartbeatService(ILogger<HeartbeatService> logger)
    {
        _logger = logger;
        _logger.LogInformation("HeartbeatService constructor called");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("HeartbeatService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Heartbeat at {Time}", DateTime.UtcNow);

            await Task.Delay(TimeSpan.FromSeconds(120), stoppingToken);
        }
    }
}