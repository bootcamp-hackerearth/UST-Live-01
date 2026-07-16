namespace HealthApp.Api.Services.Impl
{
    public class HeartbeatService : BackgroundService
    {
        private static readonly TimeSpan InitialDelay =
            TimeSpan.FromSeconds(2);

        private static readonly TimeSpan HeartbeatDelay =
            TimeSpan.FromMinutes(1);

        private readonly ILogger<HeartbeatService> _logger;

        public HeartbeatService(
            ILogger<HeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var heartbeatCount = 0L;
            var serviceStartedAtUtc = DateTime.UtcNow;

            try
            {
                await Task.Delay(InitialDelay, stoppingToken);

                serviceStartedAtUtc = DateTime.UtcNow;

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Heartbeat service started with an interval of {HeartbeatIntervalMinutes} minute(s). {EventType}",
                        HeartbeatDelay.TotalMinutes,
                        "HeartbeatServiceStarted");
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    heartbeatCount++;

                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        var serviceUptimeSeconds =
                            (DateTime.UtcNow - serviceStartedAtUtc)
                            .TotalSeconds;

                        _logger.LogDebug(
                            "API heartbeat {HeartbeatCount} completed successfully. Service uptime is {ServiceUptimeSeconds} seconds. {EventType}",
                            heartbeatCount,
                            serviceUptimeSeconds,
                            "ApiHeartbeatCompleted");
                    }

                    await Task.Delay(
                        HeartbeatDelay,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                // Expected during application shutdown. The final log entry
                // records that the service stopped normally.
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Heartbeat service failed unexpectedly after " +
                    $"{heartbeatCount} heartbeat(s).",
                    exception);
            }
            finally
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    var serviceUptimeSeconds =
                        (DateTime.UtcNow - serviceStartedAtUtc)
                        .TotalSeconds;

                    _logger.LogInformation(
                        "Heartbeat service stopped after {HeartbeatCount} heartbeat(s) and {ServiceUptimeSeconds} seconds of uptime. {EventType}",
                        heartbeatCount,
                        serviceUptimeSeconds,
                        "HeartbeatServiceStopped");
                }
            }
        }
    }
}
