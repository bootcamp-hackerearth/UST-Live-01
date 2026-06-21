using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class HealthRecordRepository(HealthAppDbContext context)
    : Repository<HealthRecord>(context), IHealthRecordRepository
{
    private IQueryable<HealthRecord> WithIncludes()
        => context.HealthRecords
            .Include(h => h.Patient)
            .Include(h => h.Doctor)
            .Include(h => h.Appointment);

    public Task<List<HealthRecord>> GetByPatientIdAsync(
        int patientId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(h => h.PatientId == patientId)
            .OrderByDescending(h => h.VisitDate)
            .ToListAsync(ct);

    public Task<List<HealthRecord>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(h => h.DoctorId == doctorId)
            .OrderByDescending(h => h.VisitDate)
            .ToListAsync(ct);

    public Task<List<HealthRecord>> GetByAppointmentIdAsync(
        int appointmentId,
        CancellationToken ct = default)
        => WithIncludes()
            .Where(h => h.AppointmentId == appointmentId)
            .ToListAsync(ct);

    public Task<bool> ExistsByAppointmentIdAsync(
        int appointmentId,
        CancellationToken ct = default)
        => context.HealthRecords
            .AnyAsync(h => h.AppointmentId == appointmentId, ct);
}