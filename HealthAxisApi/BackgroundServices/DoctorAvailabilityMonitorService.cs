using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class DoctorAvailabilityMonitorService : BackgroundService
    {
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
                    using var scope =
                        _scopeFactory.CreateScope();

                    var context =
                        scope.ServiceProvider
                            .GetRequiredService<HealthAppDbContext>();

                    var cacheService =
                        scope.ServiceProvider
                            .GetRequiredService<ICacheService>();

                    var today = DateTime.Today;

                    var doctorsToReactivate =
                        await context.Doctors
                            .Where(d =>
                                d.IsOnLeave &&
                                !context.DoctorLeaves.Any(l =>
                                    l.DoctorId == d.DoctorId &&
                                    l.EndDate.Date >= today))
                            .ToListAsync(stoppingToken);

                    if (doctorsToReactivate.Any())
                    {
                        foreach (var doctor in doctorsToReactivate)
                        {
                            doctor.IsOnLeave = false;
                        }

                        await context.SaveChangesAsync(
                            stoppingToken);

                        await cacheService.RemoveAsync(
                            "available-doctors");

                        _logger.LogInformation(
                            "{Count} doctors reactivated automatically after leave expiry.",
                            doctorsToReactivate.Count);
                    }

                    await Task.Delay(
                        TimeSpan.FromHours(24),
                        stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error while processing doctor leave expiration.");

                    await Task.Delay(
                        TimeSpan.FromMinutes(5),
                        stoppingToken);
                }
            }
        }
    }
}