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
        private readonly ILogger<AppointmentCancelledByDoctorLeaveConsumer> _logger;

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

            await _notificationService.CreateNotificationAsync(
                new CreateNotificationDto
                {
                    RecipientUserId = message.PatientUserId,
                    NotificationType = "DoctorLeave",
                    Title = "Appointment Cancelled - Doctor Unavailable",
                    Message =
                        $"Your appointment with {message.DoctorName} " +
                        $"on {message.ScheduledDate:dd-MMM-yyyy} at {message.TimeSlot} " +
                        $"was cancelled because the doctor is unavailable from " +
                        $"{message.LeaveStartDate:dd-MMM-yyyy} to " +
                        $"{message.LeaveEndDate:dd-MMM-yyyy}. " +
                        "Please book another available date.",
                    RelatedEntityId = message.AppointmentId,
                    RelatedEntityType = "Appointment"
                });

            _logger.LogInformation(
                "Doctor-leave cancellation notification created for appointment {AppointmentId} and patient {PatientId}.",
                message.AppointmentId,
                message.PatientId);
        }
    }
}