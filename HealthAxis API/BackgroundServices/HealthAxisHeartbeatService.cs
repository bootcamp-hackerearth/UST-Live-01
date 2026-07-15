namespace HealthAxis.API.BackgroundServices
{
    public class HealthAxisHeartbeatService : BackgroundService
    {
        private readonly ILogger<HealthAxisHeartbeatService> _logger;

        public HealthAxisHeartbeatService(
            ILogger<HealthAxisHeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "HealthAxis heartbeat service started at {StartedAt}",
                    DateTimeOffset.Now);
            }

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            "HealthAxis heartbeat service is running at {HeartbeatTime}",
                            DateTimeOffset.Now);
                    }

                    await Task.Delay(
                        TimeSpan.FromMinutes(8),
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {

            }

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "HealthAxis heartbeat service stopped at {StoppedAt}",
                    DateTimeOffset.Now);
            }
        }
    }
}