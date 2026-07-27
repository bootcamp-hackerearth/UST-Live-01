using HealthApp.Api.Messaging.Events;
using MassTransit;

namespace HealthApp.Api.Messaging.Publisher
{
    public class AppointmentEventPublisher : IAppointmentEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<AppointmentEventPublisher> _logger;

        public AppointmentEventPublisher(
            IPublishEndpoint publishEndpoint,
            ILogger<AppointmentEventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishAppointmentBookedAsync(
            AppointmentBookEvent appointmentEvent)
        {
            if (appointmentEvent == null)
                throw new ArgumentNullException(nameof(appointmentEvent));

            await _publishEndpoint.Publish(appointmentEvent);
        }
    }
}
