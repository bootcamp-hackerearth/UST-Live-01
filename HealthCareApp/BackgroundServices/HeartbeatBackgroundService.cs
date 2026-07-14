using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthCareApp.BackgroundServices
{
    public class HeartbeatBackgroundService : BackgroundService
    {
        private const string ServiceName = "HealthAxisHeartbeat";
        private const string StatusStarted = "Started";
        private const string StatusRunning = "Running";
        private const string StatusStopping = "Stopping";
        private const string ShutdownReason = "Application shutdown requested";

        private static readonly TimeSpan HeartbeatInterval =
            TimeSpan.FromMinutes(1);

        private readonly ILogger<HeartbeatBackgroundService> logger;

        public HeartbeatBackgroundService(
            ILogger<HeartbeatBackgroundService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            LogHeartbeatStarted();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    LogHeartbeatRunning();

                    await Task.Delay(
                        HeartbeatInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException ex)
                when (stoppingToken.IsCancellationRequested)
            {
                LogHeartbeatStopping(ex);
            }
        }

        private void LogHeartbeatStarted()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "HealthAxis heartbeat background service started. ServiceName: {ServiceName}, HeartbeatStatus: {HeartbeatStatus}, HeartbeatIntervalSeconds: {HeartbeatIntervalSeconds}",
                ServiceName,
                StatusStarted,
                HeartbeatInterval.TotalSeconds);
        }

        private void LogHeartbeatRunning()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "HealthAxis background service heartbeat. ServiceName: {ServiceName}, HeartbeatStatus: {HeartbeatStatus}",
                ServiceName,
                StatusRunning);
        }

        private void LogHeartbeatStopping(
            OperationCanceledException exception)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                exception,
                "HealthAxis heartbeat background service stopping. ServiceName: {ServiceName}, HeartbeatStatus: {HeartbeatStatus}, ShutdownReason: {ShutdownReason}",
                ServiceName,
                StatusStopping,
                ShutdownReason);
        }
    }
}