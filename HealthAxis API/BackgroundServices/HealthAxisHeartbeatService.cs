using Microsoft.Extensions.Hosting;

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
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "HealthAxis heartbeat service is running at {Time}",
                    DateTimeOffset.Now);

                await Task.Delay(
                    TimeSpan.FromSeconds(10),
                    stoppingToken);
            }
        }
    }
}