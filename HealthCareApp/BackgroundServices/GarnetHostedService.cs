using Garnet;

namespace HealthCareApp.BackgroundServices
{
    public class GarnetHostedService : IHostedService, IDisposable
    {
        private GarnetServer? server;

        private readonly ILogger<GarnetHostedService> logger;

        public GarnetHostedService(ILogger<GarnetHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                server = new GarnetServer(["--port=3278"]);

                server.Start();

                logger.LogInformation("Embedded Garnet server started on port 3278.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to start embedded Garnet server.");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            server?.Dispose();

            logger.LogInformation("Embedded Garnet server stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            server?.Dispose();
        }
    }
}