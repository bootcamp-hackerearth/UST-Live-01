using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Models;
using MassTransit;

namespace HealthAxis.API.Consumers
{
    public sealed class AppointmentBookedConsumer :
        IConsumer<AppointmentBookedEvent>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AppointmentBookedConsumer> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentBookedEvent = context.Message;

            await SaveNotificationAsync(appointmentBookedEvent);

            _logger.LogInformation(
                """
                ┌──────────────────────────────────────────────────────────────┐
                │               MASSTRANSIT EVENT CONSUMED                     │
                ├──────────────────────────────────────────────────────────────┤
                │ Event Type      : {EventType}                                |
                │ Patient ID      : {PatientId}                                |
                │ Patient Name    : {PatientName}                              |
                │ Doctor Name     : {DoctorName}                               |
                │ Doctor ID       : {DoctorId}                                 |
                │ Appointment ID  : {AppointmentId}                            |
                │ Scheduled Date  : {ScheduledDate:yyyy-MM-dd}                 |
                │ Time Slot       : {TimeSlot}                                 |
                │ Status          : {Status}                                   |
                │ Notification    : Saved to database                          |
                └──────────────────────────────────────────────────────────────┘
                """,
                appointmentBookedEvent.EventType,
                appointmentBookedEvent.PatientId,
                appointmentBookedEvent.PatientName,
                appointmentBookedEvent.DoctorName,
                appointmentBookedEvent.DoctorId,
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.ScheduledDate,
                appointmentBookedEvent.TimeSlot,
                appointmentBookedEvent.Status);
        }

        private async Task SaveNotificationAsync(
            AppointmentBookedEvent appointmentBookedEvent)
        {
            await using var scope =
                _serviceScopeFactory.CreateAsyncScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var notification = new Notification
            {
                PatientId = appointmentBookedEvent.PatientId,
                DoctorId = appointmentBookedEvent.DoctorId,
                Title = "Appointment Booked",
                Message =
                    $"Your appointment with {appointmentBookedEvent.DoctorName} " +
                    $"is booked on {appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
                    $"for {appointmentBookedEvent.TimeSlot}.",
                NotificationType = appointmentBookedEvent.EventType,
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };

            await dbContext.Notifications.AddAsync(notification);

            await dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Notification saved for AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.PatientId,
                appointmentBookedEvent.DoctorId);
        }
    }
}