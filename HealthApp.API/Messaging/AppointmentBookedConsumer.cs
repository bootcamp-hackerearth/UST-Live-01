using HealthApp.API.Data;
using HealthApp.API.Events;
using HealthApp.API.Models;
using MassTransit;

namespace HealthApp.API.Messaging;

public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
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

    public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
    {
        var appointmentBookedEvent = context.Message;

        var notification = new Notification
        {
            DoctorId = appointmentBookedEvent.DoctorId,
            AppointmentId = appointmentBookedEvent.AppointmentId,
            Title = "New Appointment Booked",
            Message =
                $"{appointmentBookedEvent.PatientName} booked an appointment on " +
                $"{appointmentBookedEvent.ScheduledDate:yyyy-MM-dd} at {appointmentBookedEvent.TimeSlot}.",
            IsRead = false,
            CreatedDate = DateTime.Now
        };

        await _dbContext.Notifications.AddAsync(
            notification,
            context.CancellationToken);

        await _dbContext.SaveChangesAsync(context.CancellationToken);

        _logger.LogInformation(
            "AppointmentBookedEvent consumed successfully. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
            appointmentBookedEvent.AppointmentId,
            appointmentBookedEvent.DoctorId);
    }
}