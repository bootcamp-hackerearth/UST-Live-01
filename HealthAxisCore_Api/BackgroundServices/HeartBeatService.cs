using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
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
            _logger.LogInformation(
                "[HEARTBEAT-SERVICE] HealthAxis heartbeat service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "[HEARTBEAT] HealthAxis API alive | Time={CurrentTime}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }

            _logger.LogInformation(
                "[HEARTBEAT-SERVICE] HealthAxis heartbeat service stopped");
        }
    }
}