using HealthApp.Api.Messaging.Events;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using MassTransit;

namespace HealthApp.Api.Messaging.Consumer
{
    public class AppointmentEventConsumer : IConsumer<AppointmentBookEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<AppointmentEventConsumer> _logger;

        public AppointmentEventConsumer(
            INotificationService notificationService,
            ILogger<AppointmentEventConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookEvent> context)
        {
            var appointmentEvent = context.Message;

            _logger.LogInformation(
                "AppointmentBookEvent consumed from RabbitMQ. AppointmentId: {AppointmentId}, Patient: {PatientName}, Doctor: {DoctorName}",
                appointmentEvent.AppointmentId,
                appointmentEvent.PatientName,
                appointmentEvent.DoctorName);

            Console.WriteLine(
                $"[RabbitMQ Consumer] Event received. AppointmentId: {appointmentEvent.AppointmentId}, Patient: {appointmentEvent.PatientName}, Doctor: {appointmentEvent.DoctorName}");

            if (string.IsNullOrWhiteSpace(appointmentEvent.DoctorIdentityUserId))
            {
                _logger.LogWarning(
                    "DoctorIdentityUserId is missing. Notification skipped. AppointmentId: {AppointmentId}",
                    appointmentEvent.AppointmentId);

                Console.WriteLine(
                    $"[RabbitMQ Consumer] DoctorIdentityUserId missing. Notification skipped. AppointmentId: {appointmentEvent.AppointmentId}");

                return;
            }

            await _notificationService.CreateAsync(new NotificationCreateDto
            {
                UserId = appointmentEvent.DoctorIdentityUserId,
                Title = "New Appointment Booked",
                Message =
                    $"New appointment booked by {appointmentEvent.PatientName} on " +
                    $"{appointmentEvent.ScheduledDate:dd-MM-yyyy} at {appointmentEvent.TimeSlot}.",
                EventType = "AppointmentBooked",
                CreatedAt = DateTime.UtcNow
            });

            _logger.LogInformation(
                "Doctor notification created. AppointmentId: {AppointmentId}, DoctorUserId: {DoctorUserId}",
                appointmentEvent.AppointmentId,
                appointmentEvent.DoctorIdentityUserId);

            Console.WriteLine(
                $"[Notification] Doctor notification created. AppointmentId: {appointmentEvent.AppointmentId}");
        }
    }
}