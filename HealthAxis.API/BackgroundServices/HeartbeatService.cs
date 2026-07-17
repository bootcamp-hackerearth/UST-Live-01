namespace HealthAxis.API.BackgroundServices
{
    public sealed class HeartbeatService : BackgroundService
    {
        private static readonly TimeSpan HeartbeatInterval =
            TimeSpan.FromSeconds(120);

        private readonly ILogger<HeartbeatService> _logger;

        public HeartbeatService(
            ILogger<HeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "HeartbeatService started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            "HealthAxis API heartbeat is running at {Time}.",
                            DateTimeOffset.Now);
                    }

                    await Task.Delay(
                        HeartbeatInterval,
                        stoppingToken);
                }
            }
            catch (OperationCanceledException exception)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    exception,
                    "HeartbeatService cancellation requested.");
            }
            finally
            {
                _logger.LogInformation(
                    "HeartbeatService stopped.");
            }
        }
    }
}