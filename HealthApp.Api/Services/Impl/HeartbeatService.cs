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

                _logger.LogInformation(
                    "Heartbeat service started with an interval of {HeartbeatIntervalMinutes} minute(s). {EventType}",
                    HeartbeatDelay.TotalMinutes,
                    "HeartbeatServiceStarted");

                while (!stoppingToken.IsCancellationRequested)
                {
                    heartbeatCount++;

                    _logger.LogDebug(
                        "API heartbeat {HeartbeatCount} completed successfully. Service uptime is {ServiceUptimeSeconds} seconds. {EventType}",
                        heartbeatCount,
                        (DateTime.UtcNow - serviceStartedAtUtc).TotalSeconds,
                        "ApiHeartbeatCompleted");

                    await Task.Delay(
                        HeartbeatDelay,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    "Heartbeat service received a shutdown signal after {HeartbeatCount} heartbeat(s). {EventType}",
                    heartbeatCount,
                    "HeartbeatServiceStopping");
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Heartbeat service stopped because of an unexpected error after {HeartbeatCount} heartbeat(s). {EventType}",
                    heartbeatCount,
                    "HeartbeatServiceFailed");

                throw;
            }
            finally
            {
                _logger.LogInformation(
                    "Heartbeat service stopped after {HeartbeatCount} heartbeat(s) and {ServiceUptimeSeconds} seconds of uptime. {EventType}",
                    heartbeatCount,
                    (DateTime.UtcNow - serviceStartedAtUtc).TotalSeconds,
                    "HeartbeatServiceStopped");
            }
        }
    }
}