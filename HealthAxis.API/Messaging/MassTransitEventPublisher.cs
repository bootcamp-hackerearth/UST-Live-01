using HealthAxis.API.Events;
using MassTransit;

namespace HealthAxis.API.Messaging
{
    public sealed class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<MassTransitEventPublisher> _logger;

        public MassTransitEventPublisher(
            IPublishEndpoint publishEndpoint,
            ILogger<MassTransitEventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishAppointmentBookedAsync(
            AppointmentBookedEvent appointmentBookedEvent,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _publishEndpoint.Publish(
                    appointmentBookedEvent,
                    cancellationToken);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        """
                        MASSTRANSIT EVENT PUBLISHED
                        ─────────────────────────────────────────
                        Event Type     : {EventType}
                        Patient Name   : {PatientName}
                        Doctor Name    : {DoctorName}
                        Doctor ID      : {DoctorId}
                        Appointment ID : {AppointmentId}
                        Scheduled Date : {ScheduledDate:yyyy-MM-dd}
                        Time Slot      : {TimeSlot}
                        Status         : {Status}
                       
                        """,
                        appointmentBookedEvent.EventType,
                        appointmentBookedEvent.PatientName,
                        appointmentBookedEvent.DoctorName,
                        appointmentBookedEvent.DoctorId,
                        appointmentBookedEvent.AppointmentId,
                        appointmentBookedEvent.ScheduledDate,
                        appointmentBookedEvent.TimeSlot,
                        appointmentBookedEvent.Status);
                }
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Appointment was saved, but AppointmentBooked event could not be published to RabbitMQ.");
            }
        }
    }
}