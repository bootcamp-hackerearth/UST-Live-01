using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class DoctorAvailabilityMonitorService : BackgroundService
    {
        private const string AvailableDoctorsCacheKey = "available-doctors";

        private static readonly TimeSpan MonitoringInterval =
            TimeSpan.FromHours(24);

        private static readonly TimeSpan RetryInterval =
            TimeSpan.FromMinutes(5);

        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ILogger<DoctorAvailabilityMonitorService> _logger;

        public DoctorAvailabilityMonitorService(
            IServiceScopeFactory scopeFactory,
            ILogger<DoctorAvailabilityMonitorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpiredDoctorLeavesAsync(
                        stoppingToken);

                    await Task.Delay(
                        MonitoringInterval,
                        stoppingToken);
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Error while processing doctor leave expiration.");

                    await DelayBeforeRetryAsync(stoppingToken);
                }
            }
        }

        private async Task ProcessExpiredDoctorLeavesAsync(
            CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var context =
                scope.ServiceProvider
                    .GetRequiredService<HealthAppDbContext>();

            var cacheService =
                scope.ServiceProvider
                    .GetRequiredService<ICacheService>();

            var today = DateTime.Today;

            var doctorsToReactivate =
                await context.Doctors
                    .Where(doctor =>
                        doctor.IsOnLeave &&
                        !context.DoctorLeaves.Any(leave =>
                            leave.DoctorId == doctor.DoctorId &&
                            leave.EndDate.Date >= today))
                    .ToListAsync(stoppingToken);

            if (doctorsToReactivate.Count == 0)
            {
                return;
            }

            foreach (var doctor in doctorsToReactivate)
            {
                doctor.IsOnLeave = false;
            }

            await context.SaveChangesAsync(stoppingToken);

            await cacheService.RemoveAsync(
                AvailableDoctorsCacheKey);

            LogReactivatedDoctors(
                doctorsToReactivate.Count);
        }

        private void LogReactivatedDoctors(int doctorCount)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "{Count} doctors reactivated automatically after leave expiry.",
                doctorCount);
        }

        private async Task DelayBeforeRetryAsync(
            CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(
                    RetryInterval,
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                // Normal application shutdown during retry delay.
            }
        }
    }
}