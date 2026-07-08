using HealthApp.Shared.Events;
using MassTransit;

namespace HealthApp.Api.Consumers
{
    public class AppointmentBookedConsumer
        : IConsumer<AppointmentBookedEvent>
    {
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(
            ILogger<AppointmentBookedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointment = context.Message;

            _logger.LogInformation(
                "\n" +
                "================ APPOINTMENT BOOKED EVENT RECEIVED ================\n" +
                " Appointment Id : {AppointmentId}\n" +
                " Patient Id     : {PatientId}\n" +
                " Patient Name   : {PatientName}\n" +
                " Doctor Id      : {DoctorId}\n" +
                " Doctor Name    : {DoctorName}\n" +
                " Scheduled Date : {ScheduledDate}\n" +
                " Time Slot      : {TimeSlot}\n" +
                "===================================================================",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.PatientName,
                appointment.DoctorId,
                appointment.DoctorName,
                appointment.ScheduledDate,
                appointment.TimeSlot);

            return Task.CompletedTask;
        }
    }
}