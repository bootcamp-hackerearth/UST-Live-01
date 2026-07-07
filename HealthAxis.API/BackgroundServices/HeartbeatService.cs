namespace HealthAxis.API.BackgroundServices
{
    public sealed class HeartbeatService : BackgroundService
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
                        "HealthAxis API heartbeat is running at {Time}.",
                        DateTimeOffset.Now);

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("HeartbeatService cancellation requested.");
            }

            _logger.LogInformation("HeartbeatService stopped.");
        }
    }
}