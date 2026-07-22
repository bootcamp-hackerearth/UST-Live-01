using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;

using MassTransit;

using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Consumers
{
    public sealed class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthAppDbContext _dbContext;

        private readonly ILogger<
            AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            HealthAppDbContext dbContext,
            ILogger<
                AppointmentBookedConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<
                AppointmentBookedEvent> context)
        {
            var appointmentEvent =
                context.Message;

            var cancellationToken =
                context.CancellationToken;

            var notificationAlreadyExists =
                await _dbContext.Notifications
                    .AsNoTracking()
                    .AnyAsync(
                        notification =>
                            notification.AppointmentId ==
                                appointmentEvent.AppointmentId,
                        cancellationToken);

            if (notificationAlreadyExists)
            {
                LogDuplicateEventIgnored(
                    appointmentEvent.EventId,
                    appointmentEvent.AppointmentId);

                return;
            }

            var notification =
                CreateNotification(
                    appointmentEvent);

            try
            {
                await _dbContext.Notifications
                    .AddAsync(
                        notification,
                        cancellationToken);

                await _dbContext.SaveChangesAsync(
                    cancellationToken);

                LogNotificationCreated(
                    appointmentEvent.EventId,
                    appointmentEvent.DoctorId,
                    appointmentEvent.AppointmentId);
            }
            catch (DbUpdateException exception)
                when (IsDuplicateNotification(exception))
            {
                /*
                 * Another consumer may have created the same
                 * notification after our initial existence check.
                 *
                 * The unique database index provides the final
                 * idempotency guarantee.
                 */
                _dbContext.Entry(notification).State =
                    EntityState.Detached;

                LogDuplicateEventIgnored(
                    appointmentEvent.EventId,
                    appointmentEvent.AppointmentId);
            }
        }

        private static Notification CreateNotification(
            AppointmentBookedEvent appointmentEvent)
        {
            return new Notification
            {
                DoctorId =
                    appointmentEvent.DoctorId,

                AppointmentId =
                    appointmentEvent.AppointmentId,

                Message =
                    $"New appointment booked by " +
                    $"{appointmentEvent.PatientName} on " +
                    $"{appointmentEvent.ScheduledDate:dd MMM yyyy} " +
                    $"at {appointmentEvent.TimeSlot}.",

                IsRead = false,

                CreatedDate =
                    DateTime.UtcNow
            };
        }

        private static bool IsDuplicateNotification(
            DbUpdateException exception)
        {
            /*
             * SQL Server duplicate-key error numbers:
             * 2601 = duplicate key in unique index
             * 2627 = violation of unique constraint
             */
            return exception.InnerException is
                Microsoft.Data.SqlClient.SqlException
                sqlException &&
                (
                    sqlException.Number == 2601 ||
                    sqlException.Number == 2627
                );
        }

        private void LogDuplicateEventIgnored(
            Guid eventId,
            int appointmentId)
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Duplicate AppointmentBookedEvent ignored. " +
                "EventId: {EventId}, " +
                "AppointmentId: {AppointmentId}",
                eventId,
                appointmentId);
        }

        private void LogNotificationCreated(
            Guid eventId,
            int doctorId,
            int appointmentId)
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "AppointmentBookedEvent consumed and " +
                "notification created. " +
                "EventId: {EventId}, " +
                "DoctorId: {DoctorId}, " +
                "AppointmentId: {AppointmentId}",
                eventId,
                doctorId,
                appointmentId);
        }
    }
}
