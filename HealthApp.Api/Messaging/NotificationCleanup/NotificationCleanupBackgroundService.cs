using HealthApp.Api.Service.Interface;

namespace HealthApp.Api.BackgroundServices
{
    public class NotificationCleanupBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationCleanupBackgroundService> _logger;

        private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(24);

        private const int DeleteReadNotificationsOlderThanDays = 30;

        public NotificationCleanupBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<NotificationCleanupBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification cleanup background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldReadNotificationsAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation(
                        "Notification cleanup background service stopped.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Notification cleanup background service failed.");
                }

                await Task.Delay(CleanupInterval, stoppingToken);
            }
        }

        private async Task CleanupOldReadNotificationsAsync(
            CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var notificationService = scope.ServiceProvider
                .GetRequiredService<INotificationService>();

            var deletedCount = await notificationService
                .CleanupOldReadNotificationsAsync(
                    DeleteReadNotificationsOlderThanDays);

            _logger.LogInformation(
                "Notification cleanup completed. Deleted {DeletedCount} read notifications older than {Days} days.",
                deletedCount,
                DeleteReadNotificationsOlderThanDays);
        }
    }
}