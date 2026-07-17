using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class HealthAxisHeartbeatService(
        ILogger<HealthAxisHeartbeatService> logger) : BackgroundService
    {
        private readonly ILogger<HealthAxisHeartbeatService> _logger = logger;

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "[HEARTBEAT-SERVICE] HealthAxis heartbeat service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "[HEARTBEAT] HealthAxis API alive | Time={CurrentTime:yyyy-MM-dd HH:mm:ss}",
                        DateTime.Now);
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(5),
                    stoppingToken);
            }

            _logger.LogInformation(
                "[HEARTBEAT-SERVICE] HealthAxis heartbeat service stopped");
        }
    }
}