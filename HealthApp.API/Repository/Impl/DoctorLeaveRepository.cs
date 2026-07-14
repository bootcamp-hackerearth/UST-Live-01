using HealthApp.API.Data;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Repository.Impl;

public class DoctorLeaveRepository(HealthAppDbContext context)
    : Repository<DoctorLeave>(context), IDoctorLeaveRepository
{
    public Task<List<DoctorLeave>> GetByDoctorIdAsync(
        int doctorId,
        CancellationToken ct = default)
        => context.DoctorLeaves
            .AsNoTracking()
            .Include(doctorLeave => doctorLeave.Doctor)
            .Where(doctorLeave => doctorLeave.DoctorId == doctorId)
            .OrderByDescending(doctorLeave => doctorLeave.StartDate)
            .ThenByDescending(doctorLeave => doctorLeave.CreatedDate)
            .ToListAsync(ct);

    public Task<bool> HasOverlappingLeaveAsync(
        int doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct = default)
        => context.DoctorLeaves.AnyAsync(
            doctorLeave =>
                doctorLeave.DoctorId == doctorId &&
                startDate.Date <= doctorLeave.EndDate.Date &&
                endDate.Date >= doctorLeave.StartDate.Date,
            ct);

    public Task<DoctorLeave?> GetLeaveForDateAsync(
        int doctorId,
        DateTime date,
        CancellationToken ct = default)
        => context.DoctorLeaves
            .AsNoTracking()
            .Include(doctorLeave => doctorLeave.Doctor)
            .FirstOrDefaultAsync(
                doctorLeave =>
                    doctorLeave.DoctorId == doctorId &&
                    date.Date >= doctorLeave.StartDate.Date &&
                    date.Date <= doctorLeave.EndDate.Date,
                ct);
}