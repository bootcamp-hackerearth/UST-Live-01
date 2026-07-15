using Garnet;

namespace HealthCare.Api.BackgroundServices;

public class GarnetHostedService : IHostedService, IDisposable
{
    private GarnetServer? _server;
    private readonly ILogger<GarnetHostedService> _logger;

    public GarnetHostedService(ILogger<GarnetHostedService> logger)
    {
        _logger = logger;
    }

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