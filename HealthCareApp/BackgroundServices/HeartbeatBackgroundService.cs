using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthCareApp.BackgroundServices
{
    public class HeartbeatBackgroundService : BackgroundService
    {
        private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromSeconds(10);

        private readonly ILogger<HeartbeatBackgroundService> logger;

        public HeartbeatBackgroundService(
            ILogger<HeartbeatBackgroundService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            logger.LogInformation(
                """
                ============================================================
                | HealthAxis Heartbeat Background Service STARTED          |
                | Purpose: Confirms hosted service is running in background |
                ============================================================
                """);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    logger.LogInformation(
                        """
                        ------------------------------------------------------------
                        | HEALTHAXIS HEARTBEAT                                     |
                        | Status : Running                                         |
                        | Time   : {Time}                                          |
                        | Note   : Background service is alive without API request |
                        ------------------------------------------------------------
                        """,
                        DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"));

                    await Task.Delay(
                        HeartbeatInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation(
                    """
                    ============================================================
                    | HealthAxis Heartbeat Background Service STOPPING         |
                    | Reason: Application shutdown requested                   |
                    ============================================================
                    """);
            }
        }
    }
}