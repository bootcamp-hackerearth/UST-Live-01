using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.BackgroundServices
{
    public sealed class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan CleanupInterval =
            TimeSpan.FromHours(1);

        private const int RetentionDays = 30;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationCleanupService> _logger;

        public NotificationCleanupService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "NotificationCleanupService started.");

            try
            {
                await RunCleanupAsync(stoppingToken);

                using var timer =
                    new PeriodicTimer(CleanupInterval);

                while (await timer.WaitForNextTickAsync(
                           stoppingToken))
                {
                    await RunCleanupAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException exception)
                when (stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    exception,
                    "NotificationCleanupService cancellation requested.");
            }
            finally
            {
                _logger.LogInformation(
                    "NotificationCleanupService stopped.");
            }
        }

        private async Task RunCleanupAsync(
       CancellationToken stoppingToken)
        {
            try
            {
                await using var scope =
                    _serviceScopeFactory.CreateAsyncScope();

                var dbContext =
                    scope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>();

                var cutoffDate =
                    DateTime.UtcNow.AddDays(-RetentionDays);

                var deletedCount =
                    await dbContext.Notifications
                        .Where(notification =>
                            notification.CreatedDate < cutoffDate)
                        .ExecuteDeleteAsync(stoppingToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Notification cleanup completed. Deleted {DeletedCount} notifications older than {CutoffDate}.",
                        deletedCount,
                        cutoffDate);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Error occurred while cleaning old notifications.");
            }
        }
    }
}