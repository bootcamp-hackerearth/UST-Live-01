using HealthCareApp.Data;
using HealthCareApp.Helpers;
using HealthCareApp.Messaging.Events;
using HealthCareApp.Models;
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

            var notificationMessage =
                $"New appointment booked by {appointmentBookedEvent.PatientName} " +
                $"on {appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
                $"at {appointmentBookedEvent.TimeSlot}.";

            var notification = new Notification
            {
                DoctorId = appointmentBookedEvent.DoctorId,
                AppointmentId = appointmentBookedEvent.AppointmentId,
                Message = notificationMessage,
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
                "AppointmentBookedEvent consumed. Notification created. DoctorId: {DoctorId}, AppointmentId: {AppointmentId}",
                appointmentBookedEvent.DoctorId,
                appointmentBookedEvent.AppointmentId);
        }
    }
}