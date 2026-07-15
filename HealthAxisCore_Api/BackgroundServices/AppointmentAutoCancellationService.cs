using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;

namespace HealthAxisCore_Api.BackgroundServices
{
    public class AppointmentAutoCancellationService(
        IServiceScopeFactory serviceScopeFactory)
        : BackgroundService
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<AppointmentAutoCancellationService>();

        private const string PendingAutoCancellationReason =
            "Auto-cancelled: appointment date passed without confirmation.";

        private const string ConfirmedAutoCancellationReason =
            "Auto-cancelled: confirmed appointment date passed without completion.";

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            Logger.Information(
                "[AUTO-CANCEL-SERVICE] Appointment date-based auto-cancellation service started");

            using var timer = new PeriodicTimer(
                TimeSpan.FromMinutes(1));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await AutoCancelOldPendingOrConfirmedAppointmentsAsync(
                        stoppingToken);
                }
                catch (Exception ex)
                {
                    Logger.Error(
                        ex,
                        "[AUTO-CANCEL-ERROR] Error occurred while scanning old pending/confirmed appointments");
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }

            Logger.Information(
                "[AUTO-CANCEL-SERVICE] Appointment date-based auto-cancellation service stopped");
        }

        private async Task AutoCancelOldPendingOrConfirmedAppointmentsAsync(
            CancellationToken ct)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var dbContext =
                scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var distributedCache =
                scope.ServiceProvider.GetRequiredService<IDistributedCache>();

            var today = DateTime.Now.Date;

            var appointmentsToArchive = await dbContext.Appointments
                .Include(appointment => appointment.Patient)
                .Include(appointment => appointment.Doctor)
                .Where(appointment =>
                    (appointment.Status == "Pending" ||
                     appointment.Status == "Confirmed") &&
                    appointment.ScheduledDate.Date < today)
                .ToListAsync(ct);

            if (appointmentsToArchive.Count == 0)
            {
                return;
            }

            var archivedCount = 0;

            foreach (var appointment in appointmentsToArchive)
            {
                var cancellationReason =
                    appointment.Status == "Confirmed"
                        ? ConfirmedAutoCancellationReason
                        : PendingAutoCancellationReason;

                var previousStatus = appointment.Status;

                var archive = new CancelledAppointmentArchive
                {
                    OriginalAppointmentId = appointment.AppointmentId,
                    PatientId = appointment.PatientId,
                    PatientName = appointment.Patient?.PatientName ?? string.Empty,
                    DoctorId = appointment.DoctorId,
                    DoctorName = appointment.Doctor?.DoctorName ?? string.Empty,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot,
                    CancellationReason = cancellationReason,
                    CancelledByRole = "System",
                    CancelledByUserId = "System",
                    CancelledAt = DateTime.UtcNow,
                    WasAutoCancelled = true,
                    ArchivedAt = DateTime.UtcNow,
                    LegalHold = false
                };

                await dbContext.CancelledAppointmentArchives.AddAsync(
                    archive,
                    ct);

                var notification = await dbContext.Notifications
                    .Where(notification =>
                        notification.AppointmentId == appointment.AppointmentId &&
                        notification.DoctorId == appointment.DoctorId &&
                        !notification.IsRead)
                    .OrderByDescending(notification => notification.CreatedAt)
                    .FirstOrDefaultAsync(ct);

                if (notification != null)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                }

                var availabilityCacheKey =
                    BuildDoctorAvailabilityCacheKey(
                        appointment.DoctorId,
                        appointment.ScheduledDate);

                await distributedCache.RemoveAsync(
                    availabilityCacheKey,
                    ct);

                dbContext.Appointments.Remove(appointment);

                archivedCount++;

                Logger.Warning(
                    "[AUTO-CANCEL-DATE-PASSED] Appointment date passed. Appointment archived and removed from active table | AppointmentId={AppointmentId} | PreviousStatus={PreviousStatus} | PatientId={PatientId} | DoctorId={DoctorId} | ScheduledDate={ScheduledDate} | Slot={TimeSlot} | Reason={Reason} | CacheKey={CacheKey}",
                    appointment.AppointmentId,
                    previousStatus,
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledDate.Date.ToString("yyyy-MM-dd"),
                    appointment.TimeSlot,
                    cancellationReason,
                    availabilityCacheKey);
            }

            if (archivedCount > 0)
            {
                await dbContext.SaveChangesAsync(ct);

                Logger.Information(
                    "[AUTO-CANCEL-SUMMARY] Old pending/confirmed appointment archival completed | ArchivedCount={ArchivedCount}",
                    archivedCount);
            }
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime scheduledDate)
        {
            return $"doctors:{doctorId}:availability:{scheduledDate:yyyy-MM-dd}";
        }
    }
}