

namespace HealthCare.Api.Services
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
            _logger.LogInformation("HeartbeatService started.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation(
                        "Heartbeat running at {Time}",
                        DateTime.Now);

                    await Task.Delay(
                        TimeSpan.FromSeconds(10),
                        stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("HeartbeatService is stopping...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in HeartbeatService.");
            }
            finally
            {
                _logger.LogInformation("HeartbeatService stopped.");
            }
        }
    }
}