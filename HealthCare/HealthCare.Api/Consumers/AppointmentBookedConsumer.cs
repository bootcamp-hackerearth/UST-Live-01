using Healthcare.Shared.Events;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HealthCare.Api.Consumers
{
    public class AppointmentBookedConsumer : IConsumer<AppointmentBookedEvent>
    {
        private readonly HealthCareDbContext _context;
        private readonly ILogger<AppointmentBookedConsumer> _logger;

        public AppointmentBookedConsumer(HealthCareDbContext context, ILogger<AppointmentBookedConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<AppointmentBookedEvent> context)
        {
            var @event = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing AppointmentBookedEvent for AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}",
                    @event.AppointmentId,
                    @event.PatientId,
                    @event.DoctorId);

                // Get patient and doctor user IDs from the database
                var appointment = await _context.Appointments.FindAsync(@event.AppointmentId);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment with ID {AppointmentId} not found", @event.AppointmentId);
                    return;
                }

                var patient = await _context.Patients.FindAsync(@event.PatientId);
                var doctor = await _context.Doctors.FindAsync(@event.DoctorId);

                if (patient == null || doctor == null)
                {
                    _logger.LogWarning("Patient or Doctor not found for AppointmentId: {AppointmentId}", @event.AppointmentId);
                    return;
                }

                // Create notification for patient
                var patientNotification = new Notification
                {
                    UserId = patient.UserId,
                    Message = $"Your appointment with Dr. {@event.DoctorName} has been booked for {FormatDate(@event.ScheduledDate)} at {@event.TimeSlot}",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                // Create notification for doctor
                var doctorNotification = new Notification
                {
                    UserId = doctor.UserId,
                    Message = $"New appointment booked with {(@event.PatientName ?? "Patient")} for {FormatDate(@event.ScheduledDate)} at {@event.TimeSlot}",
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };

                // Add notifications to database
                _context.Notifications.Add(patientNotification);
                _context.Notifications.Add(doctorNotification);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Successfully created notifications for AppointmentId: {AppointmentId}. PatientUserId: {PatientUserId}, DoctorUserId: {DoctorUserId}",
                    @event.AppointmentId,
                    patient.UserId,
                    doctor.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing AppointmentBookedEvent for AppointmentId: {AppointmentId}", @event.AppointmentId);
                throw;
            }
        }

        private static string FormatDate(DateOnly date)
        {
            return date.ToString("MMMM dd, yyyy");
        }
    }
}
