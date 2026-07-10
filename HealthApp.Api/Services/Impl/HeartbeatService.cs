namespace HealthApp.Api.Services.Impl
{
    public class HeartbeatService : BackgroundService
    {
        private static readonly TimeSpan InitialDelay =
            TimeSpan.FromSeconds(2);

        private static readonly TimeSpan HeartbeatDelay =
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
            var heartbeatCount = 0;

            try
            {
                await Task.Delay(
                    InitialDelay,
                    stoppingToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                    "\n" +
                    "==================================================\n" +
                    " HEARTBEAT SERVICE STARTED\n" +
                    " Interval       : {IntervalSeconds} seconds\n" +
                    " Started At UTC : {StartedAtUtc}\n" +
                    "==================================================",
                    HeartbeatDelay.TotalSeconds,
                    DateTime.UtcNow);
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    heartbeatCount++;
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                        "\n" +
                        "-------------------- HEARTBEAT --------------------\n" +
                        " Status          : API is alive\n" +
                        " Heartbeat Count : {HeartbeatCount}\n" +
                        " Checked At UTC  : {CheckedAtUtc}\n" +
                        "---------------------------------------------------",
                        heartbeatCount,
                        DateTime.UtcNow);
                    }

                    await Task.Delay(
                        HeartbeatDelay,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        ex,
                        "\n" +
                        "---------------------------------------------------\n" +
                        " HEARTBEAT SERVICE SHUTDOWN SIGNAL RECEIVED\n" +
                        "---------------------------------------------------");
                }
            }
            finally
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                    "\n" +
                    "==================================================\n" +
                    " HEARTBEAT SERVICE STOPPED GRACEFULLY\n" +
                    " Total Heartbeats : {HeartbeatCount}\n" +
                    " Stopped At UTC   : {StoppedAtUtc}\n" +
                    "==================================================",
                    heartbeatCount,
                    DateTime.UtcNow);
                }
            }
        }
    }
}