using HealthAxis.API.Events;

namespace HealthAxis.API.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAppointmentBookedAsync(
            AppointmentBookedEvent appointmentBookedEvent,
            CancellationToken cancellationToken = default);
    }
}