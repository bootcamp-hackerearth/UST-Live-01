using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class AppointmentRepository(HealthAppDbContext context)
    : Repository<Appointment>(context), IAppointmentRepository
{
    private IQueryable<Appointment> WithIncludes()
        => context.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Doctor);

    public Task<List<Appointment>> GetByPatientIdAsync(
        int patientId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(appointment =>
                appointment.PatientId == patientId)
            .OrderByDescending(appointment =>
                appointment.ScheduledDate)
            .ToListAsync(ct);

    public Task<List<Appointment>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(appointment =>
                appointment.DoctorId == doctorId)
            .OrderByDescending(appointment =>
                appointment.ScheduledDate)
            .ToListAsync(ct);

    public Task<List<Appointment>> GetByStatusAsync(
        AppointmentStatus status,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(appointment =>
                appointment.Status == status.ToString())
            .ToListAsync(ct);

    public Task<List<Appointment>>
        GetTodayConfirmedAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        => WithIncludes()
            .Where(appointment =>
                appointment.DoctorId == doctorId &&
                appointment.ScheduledDate.Date == DateTime.Today &&
                appointment.Status ==
                AppointmentStatus.Confirmed.ToString())
            .OrderBy(appointment =>
                appointment.TimeSlots)
            .ToListAsync(ct);

    public Task<bool> IsSlotBookedAsync(
        int doctorId,
        DateTime date,
        string timeSlot,
        CancellationToken ct = default)
        => context.Appointments.AnyAsync(
            appointment =>
                appointment.DoctorId == doctorId &&
                appointment.ScheduledDate.Date == date.Date &&
                appointment.TimeSlots == timeSlot &&
                appointment.Status !=
                AppointmentStatus.Cancelled.ToString() &&
                appointment.Status !=
                AppointmentStatus.Completed.ToString(),
            ct);

    public Task<bool>
        PatientHasActiveAppointmentWithDoctorOnDateAsync(
            int patientId,
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
        => context.Appointments.AnyAsync(
            appointment =>
                appointment.PatientId == patientId &&
                appointment.DoctorId == doctorId &&
                appointment.ScheduledDate.Date == date.Date &&
                appointment.Status !=
                AppointmentStatus.Cancelled.ToString() &&
                appointment.Status !=
                AppointmentStatus.Completed.ToString(),
            ct);

    public Task<bool>
        PatientHasActiveAppointmentOnDateAndSlotAsync(
            int patientId,
            DateTime date,
            string timeSlot,
            CancellationToken ct = default)
        => context.Appointments.AnyAsync(
            appointment =>
                appointment.PatientId == patientId &&
                appointment.ScheduledDate.Date == date.Date &&
                appointment.TimeSlots == timeSlot &&
                appointment.Status !=
                AppointmentStatus.Cancelled.ToString() &&
                appointment.Status !=
                AppointmentStatus.Completed.ToString(),
            ct);

    public Task<Appointment?> GetByIdWithDetailsAsync(
        int appointmentId,
        CancellationToken ct = default)
        => WithIncludes()
            .FirstOrDefaultAsync(
                appointment =>
                    appointment.AppointmentId == appointmentId,
                ct);

    public Task<List<Appointment>> GetAllWithDetailsAsync(
        CancellationToken ct = default)
        => WithIncludes()
            .OrderByDescending(appointment =>
                appointment.ScheduledDate)
            .ThenBy(appointment =>
                appointment.TimeSlots)
            .ToListAsync(ct);

    public Task<int> CountActiveAppointmentsInDateRangeAsync(
        int doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
    {
        var rangeStart = startDate.Date;
        var rangeEnd = endDate.Date;

        return context.Appointments.CountAsync(
            appointment =>
                appointment.DoctorId == doctorId &&
                appointment.ScheduledDate >= rangeStart &&
                appointment.ScheduledDate <= rangeEnd &&
                (
                    appointment.Status ==
                    AppointmentStatus.Pending.ToString() ||
                    appointment.Status ==
                    AppointmentStatus.Confirmed.ToString()
                ),
            ct);
    }

    public Task<List<Appointment>>
        GetActiveAppointmentsInDateRangeAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate,
            CancellationToken ct = default)
    {
        var rangeStart = startDate.Date;
        var rangeEnd = endDate.Date;

        return WithIncludes()
            .Where(appointment =>
                appointment.DoctorId == doctorId &&
                appointment.ScheduledDate >= rangeStart &&
                appointment.ScheduledDate <= rangeEnd &&
                (
                    appointment.Status ==
                    AppointmentStatus.Pending.ToString() ||
                    appointment.Status ==
                    AppointmentStatus.Confirmed.ToString()
                ))
            .OrderBy(appointment =>
                appointment.ScheduledDate)
            .ThenBy(appointment =>
                appointment.TimeSlots)
            .ToListAsync(ct);
    }
}