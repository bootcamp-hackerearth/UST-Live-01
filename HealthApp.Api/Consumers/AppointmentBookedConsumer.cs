using HealthApp.Api.Services.Interfaces;
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

            await _notificationService.CreateNotificationAsync(
                new CreateNotificationDto
                {
                    RecipientUserId = appointment.PatientUserId,
                    NotificationType = "AppointmentBooked",
                    Title = "Appointment Booked",
                    Message =
                        $"Your appointment with {appointment.DoctorName} " +
                        $"on {appointment.ScheduledDate:dd-MMM-yyyy} " +
                        $"at {appointment.TimeSlot} has been booked successfully.",
                    RelatedEntityId = appointment.AppointmentId,
                    RelatedEntityType = "Appointment"
                });

            _logger.LogInformation(
                "\n" +
                "================ NOTIFICATION CREATED FROM EVENT ================\n" +
                " Event Type      : AppointmentBookedEvent\n" +
                " Appointment Id  : {AppointmentId}\n" +
                " Patient Id      : {PatientId}\n" +
                " Patient User Id : {PatientUserId}\n" +
                " Patient Name    : {PatientName}\n" +
                " Doctor Id       : {DoctorId}\n" +
                " Doctor Name     : {DoctorName}\n" +
                " Scheduled Date  : {ScheduledDate}\n" +
                " Time Slot       : {TimeSlot}\n" +
                " Message Id      : {MessageId}\n" +
                "===============================================================",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.PatientUserId,
                appointment.PatientName,
                appointment.DoctorId,
                appointment.DoctorName,
                appointment.ScheduledDate,
                appointment.TimeSlot,
                context.MessageId);
        }
    }
}