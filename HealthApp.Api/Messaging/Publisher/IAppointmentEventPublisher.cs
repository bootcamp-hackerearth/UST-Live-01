using HealthApp.Api.Messaging.Events;

namespace HealthApp.Api.Messaging.Publisher
{
    public interface IAppointmentEventPublisher
    {
        Task PublishAppointmentBookedAsync(AppointmentBookEvent appointmentEvent);
    }
}