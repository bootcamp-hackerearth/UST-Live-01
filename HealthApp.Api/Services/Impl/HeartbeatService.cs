namespace HealthApp.Api.Services.Impl
{
    public class HeartbeatService : BackgroundService
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
            var heartbeatCount = 0;

            _logger.LogInformation(
                "\n" +
                "==================================================\n" +
                " HEARTBEAT SERVICE STARTED\n" +
                " Interval       : {IntervalSeconds} seconds\n" +
                " Started At UTC : {StartedAtUtc}\n" +
                "==================================================",
                HeartbeatInterval.TotalSeconds,
                DateTime.UtcNow);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    heartbeatCount++;

                    _logger.LogInformation(
                        "\n" +
                        "-------------------- HEARTBEAT --------------------\n" +
                        " Status          : API is alive\n" +
                        " Heartbeat Count : {HeartbeatCount}\n" +
                        " Checked At UTC  : {CheckedAtUtc}\n" +
                        "---------------------------------------------------",
                        heartbeatCount,
                        DateTime.UtcNow);

                    await Task.Delay(
                        HeartbeatInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "\n" +
                    "---------------------------------------------------\n" +
                    " HEARTBEAT SERVICE SHUTDOWN SIGNAL RECEIVED\n" +
                    "---------------------------------------------------");
            }
            finally
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