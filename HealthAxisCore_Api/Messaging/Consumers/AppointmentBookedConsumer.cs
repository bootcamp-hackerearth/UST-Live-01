using HealthAxisCore_Api.Messaging.Contracts;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interfaces;
using MassTransit;
using Serilog;

namespace HealthAxisCore_Api.Messaging.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<AppointmentBookedConsumer>();

        private readonly IRepository<Notification> _notificationRepository;

        public AppointmentBookedConsumer(
            IRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task Consume(
            ConsumeContext<AppointmentBookedEvent> context)
        {
            var appointmentBookedEvent = context.Message;

            Logger.Information(
                "[RABBITMQ-CONSUME] AppointmentBookedEvent received | AppointmentId={AppointmentId} | PatientId={PatientId} | DoctorId={DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.PatientId,
                appointmentBookedEvent.DoctorId);

            var notification = new Notification
            {
                AppointmentId = appointmentBookedEvent.AppointmentId,
                DoctorId = appointmentBookedEvent.DoctorId,
                Message =
                    $"New appointment booked by {appointmentBookedEvent.PatientName} on " +
                    $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} at {appointmentBookedEvent.TimeSlot}.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                ReadAt = null
            };

            await _notificationRepository.CreateAsync(
                notification,
                context.CancellationToken);

            Logger.Information(
                "[NOTIFICATION-CREATE] Doctor notification created | AppointmentId={AppointmentId} | DoctorId={DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);
        }
    }
}