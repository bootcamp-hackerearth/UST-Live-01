namespace HealthApp.Api.HostedServices;

public class HeartbeatService : BackgroundService
{
    private static readonly TimeSpan InitialDelay =
        TimeSpan.FromSeconds(2);

    private static readonly TimeSpan HeartbeatDelay =
        TimeSpan.FromMinutes(1);

    private readonly ILogger<HeartbeatService> _logger;

    public HeartbeatService(
        ILogger<HeartbeatService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var heartbeatCount = 0L;
        var serviceStartedAtUtc = DateTime.UtcNow;

        try
        {
            await Task.Delay(InitialDelay, stoppingToken);

            serviceStartedAtUtc = DateTime.UtcNow;
            LogServiceStarted();

            while (!stoppingToken.IsCancellationRequested)
            {
                heartbeatCount++;

                LogHeartbeat(
                    heartbeatCount,
                    serviceStartedAtUtc);

                await Task.Delay(
                    HeartbeatDelay,
                    stoppingToken);
            }
        }
        catch (OperationCanceledException)
            when (stoppingToken.IsCancellationRequested)
        {
            // Expected during application shutdown.
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                "Heartbeat service failed unexpectedly after " +
                $"{heartbeatCount} heartbeat(s).",
                exception);
        }
        finally
        {
            LogServiceStopped(
                heartbeatCount,
                serviceStartedAtUtc);
        }
    }

    private void LogServiceStarted()
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var heartbeatIntervalMinutes = HeartbeatDelay.TotalMinutes;

        _logger.LogInformation(
            "Heartbeat service started with an interval of " +
            "{HeartbeatIntervalMinutes} minute(s). " +
            "Event type: {EventType}",
            heartbeatIntervalMinutes,
            "HeartbeatServiceStarted");
    }

    private void LogHeartbeat(
        long heartbeatCount,
        DateTime serviceStartedAtUtc)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var serviceUptimeSeconds =
            (DateTime.UtcNow - serviceStartedAtUtc).TotalSeconds;

        _logger.LogInformation(
            "API heartbeat {HeartbeatCount} completed successfully. " +
            "Service uptime is {ServiceUptimeSeconds} seconds. " +
            "Event type: {EventType}",
            heartbeatCount,
            serviceUptimeSeconds,
            "ApiHeartbeatCompleted");
    }

    private void LogServiceStopped(
        long heartbeatCount,
        DateTime serviceStartedAtUtc)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        var serviceUptimeSeconds =
            (DateTime.UtcNow - serviceStartedAtUtc).TotalSeconds;

        _logger.LogInformation(
            "Heartbeat service stopped after {HeartbeatCount} " +
            "heartbeat(s) and {ServiceUptimeSeconds} seconds of uptime. " +
            "Event type: {EventType}",
            heartbeatCount,
            serviceUptimeSeconds,
            "HeartbeatServiceStopped");
    }
}