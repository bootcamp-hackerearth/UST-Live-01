namespace HealthAxisCore_Api.BackgroundServices
{
    public class HeartbeatService : BackgroundService
    {
        private readonly ILogger<HeartbeatService> _logger;

        public HeartbeatService(ILogger<HeartbeatService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "HeartbeatService started at {Time}",
                DateTimeOffset.Now
            );

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation(
                        "HealthAxis API heartbeat running at {Time}",
                        DateTimeOffset.Now
                    );
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("HeartbeatService cancellation requested.");
            }
            finally
            {
                _logger.LogInformation(
                    "HeartbeatService stopped at {Time}",
                    DateTimeOffset.Now
                );
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "HeartbeatService is stopping gracefully at {Time}",
                DateTimeOffset.Now
            );

            await base.StopAsync(cancellationToken);
        }
    }
}
