using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Models;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Consumers
{
    public sealed class AppointmentBookedConsumer :
        IConsumer<AppointmentBookedEvent>
    {
        private const int DuplicateIndexErrorNumber = 2601;

        private const int UniqueConstraintErrorNumber = 2627;

        private const string NotificationTitle =
            "New Appointment Booked";

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AppointmentBookedConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentBookedEvent = context.Message;

            var notificationCreated =
                await SaveNotificationAsync(
                    appointmentBookedEvent,
                    context.CancellationToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    """
                    MASSTRANSIT EVENT CONSUMED
                    ─────────────────────────────────────────
                    Event Type         : {EventType}
                    Patient ID         : {PatientId}
                    Patient Name       : {PatientName}
                    Doctor Name        : {DoctorName}
                    Doctor ID          : {DoctorId}
                    Appointment ID     : {AppointmentId}
                    Scheduled Date     : {ScheduledDate:yyyy-MM-dd}
                    Time Slot          : {TimeSlot}
                    Status             : {Status}
                    Notification Result: {NotificationResult}

                    """,
                    appointmentBookedEvent.EventType,
                    appointmentBookedEvent.PatientId,
                    appointmentBookedEvent.PatientName,
                    appointmentBookedEvent.DoctorName,
                    appointmentBookedEvent.DoctorId,
                    appointmentBookedEvent.AppointmentId,
                    appointmentBookedEvent.ScheduledDate,
                    appointmentBookedEvent.TimeSlot,
                    appointmentBookedEvent.Status,
                    notificationCreated
                        ? "Created"
                        : "Duplicate skipped");
            }
        }

        private async Task<bool> SaveNotificationAsync(
            AppointmentBookedEvent appointmentBookedEvent,
            CancellationToken cancellationToken)
        {
            await using var scope =
                _serviceScopeFactory.CreateAsyncScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var notificationAlreadyExists =
                await dbContext.Notifications
                    .AsNoTracking()
                    .AnyAsync(
                        notification =>
                            notification.AppointmentId ==
                            appointmentBookedEvent.AppointmentId &&
                            notification.DoctorId ==
                            appointmentBookedEvent.DoctorId &&
                            notification.NotificationType ==
                            appointmentBookedEvent.EventType,
                        cancellationToken);

            if (notificationAlreadyExists)
            {
                LogDuplicateNotification(
                    appointmentBookedEvent);

                return false;
            }

            var notification =
                CreateDoctorNotification(
                    appointmentBookedEvent);

            try
            {
                await dbContext.Notifications.AddAsync(
                    notification,
                    cancellationToken);

                await dbContext.SaveChangesAsync(
                    cancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Doctor notification created. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                        appointmentBookedEvent.AppointmentId,
                        appointmentBookedEvent.DoctorId);
                }

                return true;
            }
            catch (DbUpdateException exception)
                when (IsUniqueConstraintViolation(exception))
            {
                LogDuplicateNotification(
                    appointmentBookedEvent);

                return false;
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Notification creation failed. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}. MassTransit will handle the failure according to the configured retry policy.",
                    appointmentBookedEvent.AppointmentId,
                    appointmentBookedEvent.DoctorId);

                throw;
            }
        }

        private static Notification CreateDoctorNotification(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            return new Notification
            {
                AppointmentId =
                    appointmentBookedEvent.AppointmentId,

                PatientId = null,

                DoctorId =
                    appointmentBookedEvent.DoctorId,

                Title =
                    NotificationTitle,

                Message =
                    $"New appointment booked by " +
                    $"{appointmentBookedEvent.PatientName} " +
                    $"on " +
                    $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
                    $"at {appointmentBookedEvent.TimeSlot}.",

                NotificationType =
                    appointmentBookedEvent.EventType,

                IsRead =
                    false,

                CreatedDate =
                    DateTime.UtcNow
            };
        }

        private void LogDuplicateNotification(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Duplicate AppointmentBookedEvent safely skipped. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);
        }

        private static bool IsUniqueConstraintViolation(
            DbUpdateException exception)
        {
            if (exception.InnerException
                is not SqlException sqlException)
            {
                return false;
            }

            return sqlException.Number ==
                       DuplicateIndexErrorNumber ||
                   sqlException.Number ==
                       UniqueConstraintErrorNumber;
        }
    }
}