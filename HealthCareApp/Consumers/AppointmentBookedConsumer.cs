using HealthCareApp.Data;
using HealthCareApp.Models;
using HealthCareApp.Shared.Events;
using MassTransit;

namespace HealthCareApp.Consumers
{
    public class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthAxisDbContext _db;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            HealthAxisDbContext db, ILogger<AppointmentBookedConsumer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var msg = context.Message;

            _logger.LogInformation("AppointmentBookedEvent received. AppointmentId={AppointmentId}, DoctorId={DoctorId}, Patient={PatientName}",msg.AppointmentId,msg.DoctorId,msg.PatientName);
            

            var notification = new Notification
            {
                DoctorId = msg.DoctorId,
                CreatedDate = DateTime.UtcNow,
                IsRead = false,
                Message =
                    $"New appointment booked by {msg.PatientName} on {msg.ScheduledDate:yyyy-MM-dd} at {msg.TimeSlot}"
            };

            _db.Notifications.Add(notification);

            await _db.SaveChangesAsync();

            _logger.LogInformation("Notification created successfully for DoctorId={DoctorId}",msg.DoctorId);
        }
    }
}