using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Events;
using MassTransit;

namespace HealthApp.Api.Consumers
{
    public class AppointmentCancelledByDoctorLeaveConsumer
        : IConsumer<AppointmentCancelledByDoctorLeaveEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<AppointmentCancelledByDoctorLeaveConsumer>
            _logger;

        public AppointmentCancelledByDoctorLeaveConsumer(
            INotificationService notificationService,
            ILogger<AppointmentCancelledByDoctorLeaveConsumer> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Consume(
            ConsumeContext<AppointmentCancelledByDoctorLeaveEvent> context)
        {
            var message = context.Message;

            try
            {
                await _notificationService.CreateNotificationAsync(
                    new CreateNotificationDto
                    {
                        RecipientUserId = message.PatientUserId,
                        NotificationType = "DoctorLeave",
                        Title = "Appointment Cancelled - Doctor Unavailable",
                        Message =
                            $"Your appointment with {message.DoctorName} " +
                            $"on {message.ScheduledDate:dd MMM yyyy} at " +
                            $"{message.TimeSlot} was cancelled because the " +
                            "doctor is unavailable from " +
                            $"{message.LeaveStartDate:dd MMM yyyy} to " +
                            $"{message.LeaveEndDate:dd MMM yyyy}. " +
                            "Please book another available date.",
                        RelatedEntityId = message.AppointmentId,
                        RelatedEntityType = "Appointment"
                    },
                    context.CancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Doctor-leave cancellation notification created for appointment {AppointmentId}, patient {PatientId}, doctor {DoctorId}, and message {MessageId}. Event type: {EventType}",
                        message.AppointmentId,
                        message.PatientId,
                        message.DoctorId,
                        context.MessageId,
                        "DoctorLeaveCancellationNotificationCreated");
                }
            }
            catch (OperationCanceledException)
                when (context.CancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to create doctor-leave cancellation notification for appointment {AppointmentId}, patient {PatientId}, doctor {DoctorId}, and message {MessageId}. Event type: {EventType}",
                    message.AppointmentId,
                    message.PatientId,
                    message.DoctorId,
                    context.MessageId,
                    "DoctorLeaveCancellationNotificationFailed");

                throw;
            }
        }
    }
}