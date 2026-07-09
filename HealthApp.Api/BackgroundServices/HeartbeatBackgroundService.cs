using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthApp.Api.BackgroundServices 
{ 
    public class HeartbeatBackgroundService : BackgroundService
    {
        private readonly ILogger<HeartbeatBackgroundService> logger;
        public HeartbeatBackgroundService(ILogger<HeartbeatBackgroundService> logger)
        {
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("HealthAxis Heartbeat Background Service started.+++++++++++++++");
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("HealthAxis heartbeat running at {Time}----------------",
                    DateTime.Now);
                await Task.Delay(TimeSpan.FromSeconds(10),stoppingToken);
            }
            logger.LogInformation("HealthAxis Heartbeat Background Service stopped.***************");
        }
    }
}