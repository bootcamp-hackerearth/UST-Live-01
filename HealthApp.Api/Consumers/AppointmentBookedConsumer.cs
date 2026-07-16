using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Events;
using MassTransit;

namespace HealthApp.Api.Consumers
{
    public class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly ILogger<AppointmentBookedConsumer> _logger;
        private readonly INotificationService _notificationService;

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
            var appointment = context.Message;

            try
            {
                await _notificationService.CreateNotificationAsync(
                    new CreateNotificationDto
                    {
                        RecipientUserId = appointment.PatientUserId,
                        NotificationType = "AppointmentBooked",
                        Title = "Appointment Booked",
                        Message =
                            $"Your appointment with {appointment.DoctorName} " +
                            $"on {appointment.ScheduledDate:dd MMM yyyy} " +
                            $"at {appointment.TimeSlot} has been booked " +
                            "successfully.",
                        RelatedEntityId = appointment.AppointmentId,
                        RelatedEntityType = "Appointment"
                    },
                    context.CancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Appointment booking notification created for appointment {AppointmentId}, patient {PatientId}, doctor {DoctorId}, and message {MessageId}. Event type: {EventType}",
                        appointment.AppointmentId,
                        appointment.PatientId,
                        appointment.DoctorId,
                        context.MessageId,
                        "AppointmentBookingNotificationCreated");
                }
            }
            catch (OperationCanceledException)
                when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Failed to create appointment booking notification for " +
                    $"appointment {appointment.AppointmentId}, " +
                    $"patient {appointment.PatientId}, " +
                    $"doctor {appointment.DoctorId}, and " +
                    $"message {context.MessageId}.",
                    exception);
            }
        }
    }
}