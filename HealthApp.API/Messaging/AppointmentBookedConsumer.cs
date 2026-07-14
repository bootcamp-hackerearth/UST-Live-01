using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.API.Models;
using MassTransit;

namespace HealthApp.API.Messaging;

public class AppointmentBookedConsumer
    : IConsumer<AppointmentBookedEvent>
{
    private readonly HealthAppDbContext _dbContext;
    private readonly ILogger<AppointmentBookedConsumer> _logger;

    public AppointmentBookedConsumer(
        HealthAppDbContext dbContext,
        ILogger<AppointmentBookedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(
        ConsumeContext<AppointmentBookedEvent> context)
    {
        var appointmentBookedEvent = context.Message;

        _logger.LogInformation(
            "AppointmentBookedEvent received from MassTransit/RabbitMQ. " +
            "MessageId: {MessageId}, AppointmentId: {AppointmentId}, " +
            "DoctorId: {DoctorId}, PatientName: {PatientName}, " +
            "ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
            context.MessageId,
            appointmentBookedEvent.AppointmentId,
            appointmentBookedEvent.DoctorId,
            appointmentBookedEvent.PatientName,
            appointmentBookedEvent.ScheduledDate,
            appointmentBookedEvent.TimeSlot);

        try
        {
            var notification = new Notification
            {
                DoctorId = appointmentBookedEvent.DoctorId,
                AppointmentId =
                    appointmentBookedEvent.AppointmentId,
                Title = "New Appointment Booked",
                Message =
                    $"{appointmentBookedEvent.PatientName} " +
                    $"booked an appointment on " +
                    $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} " +
                    $"at {appointmentBookedEvent.TimeSlot}.",
                IsRead = false,
                CreatedDate = DateTime.Now
            };

            _logger.LogInformation(
                "Creating doctor notification from " +
                "AppointmentBookedEvent. AppointmentId: {AppointmentId}, " +
                "DoctorId: {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);

            await _dbContext.Notifications.AddAsync(
                notification,
                context.CancellationToken);

            await _dbContext.SaveChangesAsync(
                context.CancellationToken);

            _logger.LogInformation(
                "Doctor notification saved successfully in " +
                "Notifications table. NotificationId: {NotificationId}, " +
                "AppointmentId: {AppointmentId}, DoctorId: {DoctorId}, " +
                "IsRead: {IsRead}",
                notification.NotificationId,
                notification.AppointmentId,
                notification.DoctorId,
                notification.IsRead);

            _logger.LogInformation(
                "AppointmentBookedEvent consumed successfully. " +
                "MessageId: {MessageId}, AppointmentId: {AppointmentId}, " +
                "DoctorId: {DoctorId}, NotificationId: {NotificationId}",
                context.MessageId,
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId,
                notification.NotificationId);
        }
        catch (OperationCanceledException)
            when (context.CancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning(
                "AppointmentBookedEvent processing was cancelled. " +
                "MessageId: {MessageId}, AppointmentId: {AppointmentId}, " +
                "DoctorId: {DoctorId}",
                context.MessageId,
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process AppointmentBookedEvent or save " +
                "doctor notification. MessageId: {MessageId}, " +
                "AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                context.MessageId,
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);

            throw;
        }
    }
}