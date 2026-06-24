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
            .Include(a => a.Patient)
            .Include(a => a.Doctor);

    public Task<List<Appointment>> GetByPatientIdAsync(
        int patientId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.ScheduledDate)
            .ToListAsync(ct);

    public Task<List<Appointment>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(a => a.DoctorId == doctorId)
            .OrderByDescending(a => a.ScheduledDate)
            .ToListAsync(ct);

    public Task<List<Appointment>> GetByStatusAsync(
        AppointmentStatus status,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(a => a.Status == status.ToString())
            .ToListAsync(ct);

    public Task<List<Appointment>> GetTodayConfirmedAppointmentsByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate.Date == DateTime.Today &&
                a.Status == AppointmentStatus.Confirmed.ToString())
            .OrderBy(a => a.TimeSlots)
            .ToListAsync(ct);

    public Task<bool> IsSlotBookedAsync(
        int doctorId,
        DateTime date,
        string timeSlot,
        CancellationToken ct = default)
        => context.Appointments.AnyAsync(a =>
            a.DoctorId == doctorId &&
            a.ScheduledDate.Date == date.Date &&
            a.TimeSlots == timeSlot &&
            a.Status != AppointmentStatus.Cancelled.ToString() &&
            a.Status != AppointmentStatus.Completed.ToString(), ct);

    public Task<bool> PatientHasActiveAppointmentWithDoctorOnDateAsync(
        int patientId,
        int doctorId,
        DateTime date,
        CancellationToken ct = default)
        => context.Appointments.AnyAsync(a =>
            a.PatientId == patientId &&
            a.DoctorId == doctorId &&
            a.ScheduledDate.Date == date.Date &&
            a.Status != AppointmentStatus.Cancelled.ToString() &&
            a.Status != AppointmentStatus.Completed.ToString(), ct);

    public Task<bool> PatientHasActiveAppointmentOnDateAndSlotAsync(
        int patientId,
        DateTime date,
        string timeSlot,
        CancellationToken ct = default)
        => context.Appointments.AnyAsync(a =>
            a.PatientId == patientId &&
            a.ScheduledDate.Date == date.Date &&
            a.TimeSlots == timeSlot &&
            a.Status != AppointmentStatus.Cancelled.ToString() &&
            a.Status != AppointmentStatus.Completed.ToString(), ct);

    public Task<Appointment?> GetByIdWithDetailsAsync(
            int appointmentId,
            CancellationToken ct = default)
            => WithIncludes()
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, ct);

    public Task<List<Appointment>> GetAllWithDetailsAsync(
            CancellationToken ct = default)
            => WithIncludes()
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlots)
                .ToListAsync(ct);
}