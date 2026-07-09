using HealthAxis.API.Data;
using HealthAxis.API.Events;
using HealthAxis.API.Models;
using MassTransit;

namespace HealthAxis.API.Consumers;

public sealed class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
{
    private readonly HealthAxisDbContext _dbContext;
    private readonly ILogger<AppointmentBookedConsumer> _logger;

    public AppointmentBookedConsumer(
        HealthAxisDbContext dbContext,
        ILogger<AppointmentBookedConsumer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
    {
        var appointmentEvent = context.Message;

        var formattedEventMessage =
            "===============================" + Environment.NewLine +
            "APPOINTMENT EVENT RECEIVED" + Environment.NewLine +
            "===============================" + Environment.NewLine +
            $"Event Type  : {appointmentEvent.EventType}" + Environment.NewLine +
            $"Appointment : {appointmentEvent.AppointmentId}" + Environment.NewLine +
            $"Patient     : {appointmentEvent.PatientName}" + Environment.NewLine +
            $"Doctor      : {appointmentEvent.DoctorId}" + Environment.NewLine +
            $"Date        : {appointmentEvent.ScheduledDate:dd-MMM-yyyy}" + Environment.NewLine +
            $"Time Slot   : {appointmentEvent.TimeSlot}" + Environment.NewLine +
            $"Occurred At : {appointmentEvent.OccurredAt:dd-MMM-yyyy HH:mm:ss}" + Environment.NewLine +
            "===============================";

        _logger.LogInformation("{AppointmentEventMessage}", formattedEventMessage);

        var notification = new Notification
        {
            DoctorId = appointmentEvent.DoctorId,
            Message = $"New appointment booked by {appointmentEvent.PatientName} on {appointmentEvent.ScheduledDate:dd-MMM-yyyy} at {appointmentEvent.TimeSlot}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Notifications.AddAsync(notification, context.CancellationToken);
        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation(
            "Notification created for DoctorId {DoctorId} for AppointmentId {AppointmentId}",
            appointmentEvent.DoctorId,
            appointmentEvent.AppointmentId);
    }
}