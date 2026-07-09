using HealthApp.Api.Messaging.Events;

namespace HealthApp.Api.Messaging.Publisher
{
    public class AppointmentEventPublisher : IAppointmentEventPublisher
    {
        private readonly ILogger<AppointmentEventPublisher> _logger;

        public AppointmentEventPublisher(
            ILogger<AppointmentEventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAppointmentBookedAsync(
            AppointmentBookEvent appointmentEvent)
        {
            _logger.LogInformation(
                "New appointment booked. AppointmentId: {AppointmentId}, Patient: {PatientName}, Doctor: {DoctorName}, Date: {ScheduledDate}, Slot: {TimeSlot}",
                appointmentEvent.AppointmentId,
                appointmentEvent.PatientName,
                appointmentEvent.DoctorName,
                appointmentEvent.ScheduledDate,
                appointmentEvent.TimeSlot
            );

            return Task.CompletedTask;
        }
    }

}
