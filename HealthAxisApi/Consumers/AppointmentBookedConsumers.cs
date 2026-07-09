using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using MassTransit;

namespace HealthAxisCore_Api.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthAppDbContext _dbContext;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            HealthAppDbContext dbContext,
            ILogger<AppointmentBookedConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentEvent = context.Message;

            var notification = new Notification
            {
                DoctorId = appointmentEvent.DoctorId,
                AppointmentId = appointmentEvent.AppointmentId,
                Message = $"New appointment booked by {appointmentEvent.PatientName} on {appointmentEvent.ScheduledDate:dd MMM yyyy} at {appointmentEvent.TimeSlot}.",
                IsRead = false,
                CreatedDate = DateTime.Now
            };

            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "AppointmentBookedEvent consumed. Notification created. DoctorId: {DoctorId}, AppointmentId: {AppointmentId}",
                appointmentEvent.DoctorId,
                appointmentEvent.AppointmentId
            );
        }
    }
}