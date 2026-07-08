using HealthAxis.API.Data;
using HealthAxis.API.Messages;
using HealthAxis.API.Models;
using MassTransit;

namespace HealthAxis.API.Consumers
{
    public class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthAxisDbContext _dbContext;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            HealthAxisDbContext dbContext,
            ILogger<AppointmentBookedConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(
     ConsumeContext<AppointmentBookedEvent> context)
        {
            AppointmentBookedEvent message = context.Message;

            Notification notification = new()
            {
                DoctorId = message.DoctorId,
                Message =
                    $"New appointment booked by {message.PatientName} on {message.ScheduledDate:dd MMM yyyy} at {message.TimeSlot}.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Notifications.Add(notification);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                """

        ==========================
        APPOINTMENT EVENT RECEIVED
        ==========================

        Event Type  : AppointmentBookedEvent
        Appointment : {AppointmentId}
        Patient     : {PatientName}
        Doctor      : {DoctorId}
        Date        : {ScheduledDate}
        Time Slot   : {TimeSlot}
        Received At : {ReceivedTime}

        ==========================

        """,
                message.AppointmentId,
                message.PatientName,
                message.DoctorId,
                message.ScheduledDate.ToString("dd/MM/yyyy"),
                message.TimeSlot,
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }
    }
}