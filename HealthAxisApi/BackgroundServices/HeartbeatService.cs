using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
{
    public sealed class HeartbeatService : BackgroundService
    {
        private static readonly TimeSpan HeartbeatInterval =
            TimeSpan.FromSeconds(10);

        private readonly ILogger<HeartbeatService> _logger;

        public HeartbeatService(
            ILogger<HeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            LogServiceStarted();

            using var timer =
                new PeriodicTimer(HeartbeatInterval);

            try
            {
                while (await timer.WaitForNextTickAsync(
                    stoppingToken))
                {
                    LogHeartbeat();
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                LogCancellationRequested();
            }
            finally
            {
                LogServiceStopped();
            }
        }

        public override async Task StopAsync(
            CancellationToken cancellationToken)
        {
            LogServiceStopping();

            await base.StopAsync(cancellationToken);
        }

        private void LogServiceStarted()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "HeartbeatService started at {Time}",
                DateTimeOffset.Now);
        }

        private void LogHeartbeat()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "HealthAxis API heartbeat running at {Time}",
                DateTimeOffset.Now);
        }

        private void LogCancellationRequested()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "HeartbeatService cancellation requested.");
        }

        private void LogServiceStopped()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "HeartbeatService stopped at {Time}",
                DateTimeOffset.Now);
        }

        private void LogServiceStopping()
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "HeartbeatService is stopping gracefully at {Time}",
                DateTimeOffset.Now);
        }
    }
}