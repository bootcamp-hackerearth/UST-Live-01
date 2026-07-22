using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.API.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

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
        var cancellationToken = context.CancellationToken;

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
            /*
             * Idempotency check:
             * Check whether a notification has already been created
             * for the same appointment and doctor.
             */
            var notificationAlreadyExists =
                await _dbContext.Notifications
                    .AsNoTracking()
                    .AnyAsync(
                        notification =>
                            notification.AppointmentId ==
                                appointmentBookedEvent.AppointmentId &&
                            notification.DoctorId ==
                                appointmentBookedEvent.DoctorId,
                        cancellationToken);

            if (notificationAlreadyExists)
            {
                _logger.LogInformation(
                    "Duplicate AppointmentBookedEvent skipped because " +
                    "a notification already exists. " +
                    "MessageId: {MessageId}, AppointmentId: {AppointmentId}, " +
                    "DoctorId: {DoctorId}",
                    context.MessageId,
                    appointmentBookedEvent.AppointmentId,
                    appointmentBookedEvent.DoctorId);

                return;
            }

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
                cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Doctor notification saved successfully. " +
                "NotificationId: {NotificationId}, " +
                "AppointmentId: {AppointmentId}, " +
                "DoctorId: {DoctorId}, IsRead: {IsRead}",
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
            when (cancellationToken.IsCancellationRequested)
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