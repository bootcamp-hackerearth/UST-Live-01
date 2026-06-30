using HealthAxis.API.Data;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories.Implementations;

public sealed class AppointmentRepository
    : Repository<Appointment>, IAppointmentRepository
{
    private readonly ApplicationDbContext context;

    public AppointmentRepository(ApplicationDbContext context)
        : base(context)
    {
        this.context = context;
    }

    public async Task<bool> HasDoctorSlotConflictAsync(
        int doctorId,
        DateTime scheduledDate,
        string timeSlot)
    {
        DateTime appointmentDate = scheduledDate.Date;
        string cleanTimeSlot = timeSlot.Trim();

        return await context.Appointments.AnyAsync(appointment =>
            appointment.DoctorId == doctorId &&
            appointment.ScheduledDate.Date == appointmentDate &&
            appointment.TimeSlot == cleanTimeSlot &&
            (
                appointment.Status == AppointmentStatus.Pending ||
                appointment.Status == AppointmentStatus.Confirmed
            ));
    }

    public async Task<bool> HasPatientSlotConflictAsync(
        int patientId,
        DateTime scheduledDate,
        string timeSlot)
    {
        DateTime appointmentDate = scheduledDate.Date;
        string cleanTimeSlot = timeSlot.Trim();

        return await context.Appointments.AnyAsync(appointment =>
            appointment.PatientId == patientId &&
            appointment.ScheduledDate.Date == appointmentDate &&
            appointment.TimeSlot == cleanTimeSlot &&
            (
                appointment.Status == AppointmentStatus.Pending ||
                appointment.Status == AppointmentStatus.Confirmed
            ));
    }
}