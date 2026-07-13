using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class DoctorLeaveRepository
        : Repository<DoctorLeave>, IDoctorLeaveRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorLeaveRepository(HealthAppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DoctorLeave>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.DoctorLeaves
                .AsNoTracking()
                .Include(leave => leave.Doctor)
                .Where(leave => leave.DoctorId == doctorId)
                .OrderByDescending(leave => leave.StartDate)
                .ThenByDescending(leave => leave.CreatedAtUtc)
                .ToListAsync(ct);
        }

        public async Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken ct = default)
        {
            return await _context.DoctorLeaves
                .AsNoTracking()
                .AnyAsync(
                    leave =>
                        leave.DoctorId == doctorId &&
                        leave.StartDate <= endDate &&
                        leave.EndDate >= startDate,
                    ct);
        }

        public async Task<DoctorLeave?> GetLeaveForDateAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            return await _context.DoctorLeaves
                .AsNoTracking()
                .Include(leave => leave.Doctor)
                .Where(leave =>
                    leave.DoctorId == doctorId &&
                    leave.StartDate <= date &&
                    leave.EndDate >= date)
                .OrderByDescending(leave => leave.CreatedAtUtc)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            return await _context.DoctorLeaves
                .AsNoTracking()
                .AnyAsync(
                    leave =>
                        leave.DoctorId == doctorId &&
                        leave.StartDate <= date &&
                        leave.EndDate >= date,
                    ct);
        }
    }
}