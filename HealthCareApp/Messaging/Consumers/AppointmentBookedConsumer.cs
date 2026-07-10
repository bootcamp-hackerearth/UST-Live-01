using HealthCareApp.Data;
using HealthCareApp.Helpers;
using HealthCareApp.Messaging.Events;
using HealthCareApp.Models;
using HealthCareApp.Shared.Enums;
using MassTransit;

namespace HealthCareApp.Messaging.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
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

            string notificationMessage =
                $"New appointment booked by {appointmentBookedEvent.PatientName} " +
                $"on {appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
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

            logger.LogInformation(
                "AppointmentBookedEvent consumed and notification created. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);
        }
    }
}