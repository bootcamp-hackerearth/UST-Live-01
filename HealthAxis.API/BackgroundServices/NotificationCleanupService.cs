using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HealthAxis.API.BackgroundServices
{
    public sealed class NotificationCleanupService : BackgroundService
    {
        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(1);

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<NotificationCleanupService> _logger;

        public NotificationCleanupService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<NotificationCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationCleanupService started.");

            await RunCleanupAsync(stoppingToken);

            using var timer = new PeriodicTimer(CleanupInterval);

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await RunCleanupAsync(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation(
                    "NotificationCleanupService cancellation requested.");
            }

            _logger.LogInformation("NotificationCleanupService stopped.");
        }

        private async Task RunCleanupAsync(CancellationToken stoppingToken)
        {
            try
            {
                await using var scope =
                    _serviceScopeFactory.CreateAsyncScope();

                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                var cutoffDate = DateTime.UtcNow.AddDays(-30);

                var deletedCount = await dbContext.Notifications
                    .Where(notification => notification.CreatedDate < cutoffDate)
                    .ExecuteDeleteAsync(stoppingToken);

                _logger.LogInformation(
                    "Notification cleanup completed. Deleted {DeletedCount} notifications older than {CutoffDate}.",
                    deletedCount,
                    cutoffDate);
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