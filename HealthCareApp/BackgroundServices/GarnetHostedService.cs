using Garnet;

namespace HealthCareApp.BackgroundServices
{
    public sealed class GarnetHostedService : IHostedService, IDisposable
    {
        private const string GarnetPortArgument = "--port=3278";
        private const int GarnetPort = 3278;

        private GarnetServer? server;

        private bool disposed;

        private readonly ILogger<GarnetHostedService> logger;

        public GarnetHostedService(
            ILogger<GarnetHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(
            CancellationToken cancellationToken)
        {
            try
            {
                server = new GarnetServer([GarnetPortArgument]);

                server.Start();

                LogGarnetServerStarted();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to start embedded Garnet server.");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(
            CancellationToken cancellationToken)
        {
            DisposeServer();

            LogGarnetServerStopped();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                DisposeServer();
            }

            disposed = true;
        }

        private void DisposeServer()
        {
            server?.Dispose();

            server = null;
        }

        private void LogGarnetServerStarted()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Embedded Garnet server started on port {Port}.",
                GarnetPort);
        }

        private void LogGarnetServerStopped()
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Embedded Garnet server stopped.");
        }
    }
}