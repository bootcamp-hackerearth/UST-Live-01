using HealthAxis.API.Events;
using HealthAxis.API.Services.Interfaces;
using MassTransit;

namespace HealthAxis.API.Consumers
{
    public sealed class AppointmentBookedConsumer :
        IConsumer<AppointmentBookedEvent>
    {
        private readonly INotificationService
            _notificationService;

        private readonly ILogger<AppointmentBookedConsumer>
            _logger;

        public AppointmentBookedConsumer(
            INotificationService notificationService,
            ILogger<AppointmentBookedConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentBookedEvent = context.Message;

            var notificationCreated =
                await _notificationService
                    .CreateAppointmentBookedNotificationAsync(
                        appointmentBookedEvent,
                        context.CancellationToken);

            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

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
}