using Garnet;

namespace HealthCare.Api.BackgroundServices;

public sealed class GarnetHostedService : IHostedService, IDisposable
{
    private GarnetServer? _server;
    private readonly ILogger<GarnetHostedService> _logger;
    private bool _disposed;

    public GarnetHostedService(
        ILogger<GarnetHostedService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(
        CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try
        {
            _server = new GarnetServer(
                ["--port=6379"]);

            _server.Start();

            _logger.LogInformation(
                "Embedded Garnet server started on port 6379");
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to start embedded Garnet server");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        DisposeServer();

        _logger.LogInformation(
            "Embedded Garnet server stopped");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(disposing: true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DisposeServer();
        }

        _disposed = true;
    }

    private void DisposeServer()
    {
        _server?.Dispose();
        _server = null;
    }
}