using Garnet;

namespace HealthAxisCore_Api.BackgroundServices;

public class GarnetHostedService(ILogger<GarnetHostedService> logger) : IHostedService, IDisposable
{
    private GarnetServer? _server;
    private readonly ILogger<GarnetHostedService> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _server = new GarnetServer(["--port=6379"]);
            _server.Start();
            _logger.LogInformation("Embedded Garnet server started on port 6379");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start embedded Garnet server");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _server?.Dispose();
        _logger.LogInformation("Embedded Garnet server stopped");
        return Task.CompletedTask;
    }

    public void Dispose() => _server?.Dispose();
}
