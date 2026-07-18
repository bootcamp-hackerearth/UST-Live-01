using HealthCareApp.Data;
using HealthCareApp.Helpers;
using HealthCareApp.Messaging.Events;
using HealthCareApp.Models;
using HealthCareApp.Shared.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Messaging.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string AppointmentBookedEventType = "AppointmentBooked";
        private const string EventStageNotificationCreated = "NotificationCreated";
        private const string EventStageDuplicateSkipped = "DuplicateSkipped";

        private readonly HealthAxisDbContext dbContext;

        private readonly ILogger<AppointmentBookedConsumer> logger;

        public AppointmentBookedConsumer(
            HealthAxisDbContext dbContext,
            ILogger<AppointmentBookedConsumer> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentBookedEvent = context.Message;

            var notificationAlreadyExists =
                await dbContext.Notifications.AnyAsync(
                    notification =>
                        notification.AppointmentId == appointmentBookedEvent.AppointmentId &&
                        notification.NotificationType == NotificationType.AppointmentBooked,
                    context.CancellationToken);

            if (notificationAlreadyExists)
            {
                LogDuplicateAppointmentBookedNotificationSkipped(
                    appointmentBookedEvent);

                return;
            }

            string notificationMessage =
                $"New appointment booked by {appointmentBookedEvent.PatientName} " +
                $"on {FormatDate(appointmentBookedEvent.ScheduledDate)} " +
                $"at {appointmentBookedEvent.TimeSlot}.";

            var notification = new Notification
            {
                PatientId = null,
                DoctorId = appointmentBookedEvent.DoctorId,
                AppointmentId = appointmentBookedEvent.AppointmentId,
                Title = "New appointment booked",
                Message = notificationMessage,
                NotificationType = NotificationType.AppointmentBooked,
                IsRead = false,
                CreatedDate = DateTime.Now
            };

            dbContext.Notifications.Add(notification);

            await dbContext.SaveChangesAsync(context.CancellationToken);

            ConsoleHighlightHelper.WriteNotificationBox(
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId,
                notificationMessage);

            LogAppointmentBookedNotificationCreated(
                appointmentBookedEvent);
        }

        private void LogAppointmentBookedNotificationCreated(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            using var appointmentBookedEventLogScope =
                BeginAppointmentBookedEventLogScope(
                    appointmentBookedEvent,
                    EventStageNotificationCreated);

            logger.LogInformation(
                "Appointment booked event consumed and doctor notification created. EventStage: {EventStage}",
                EventStageNotificationCreated);
        }

        private void LogDuplicateAppointmentBookedNotificationSkipped(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            using var appointmentBookedEventLogScope =
                BeginAppointmentBookedEventLogScope(
                    appointmentBookedEvent,
                    EventStageDuplicateSkipped);

            logger.LogInformation(
                "Duplicate appointment booked event skipped because notification already exists. EventStage: {EventStage}",
                EventStageDuplicateSkipped);
        }

        private IDisposable? BeginAppointmentBookedEventLogScope(
            AppointmentBookedEvent appointmentBookedEvent,
            string eventStage)
        {
            return logger.BeginScope(new Dictionary<string, object>
            {
                ["EventType"] = AppointmentBookedEventType,
                ["EventStage"] = eventStage,
                ["AppointmentId"] = appointmentBookedEvent.AppointmentId,
                ["DoctorId"] = appointmentBookedEvent.DoctorId,
                ["ScheduledDate"] = FormatDate(appointmentBookedEvent.ScheduledDate),
                ["TimeSlot"] = appointmentBookedEvent.TimeSlot
            });
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }
    }
}