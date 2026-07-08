using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class HealthAxisHeartbeatService : BackgroundService
    {
        private readonly ILogger<HealthAxisHeartbeatService> _logger;

        public HealthAxisHeartbeatService(ILogger<HealthAxisHeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("HealthAxis heartbeat background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "HealthAxis heartbeat running at {CurrentTime}",
                    DateTime.Now);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("HealthAxis heartbeat background service stopped.");
        }
    }
}