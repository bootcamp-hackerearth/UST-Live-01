using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Repositories.Implementation
{
    public class DoctorLeaveRepository : IDoctorLeaveRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorLeaveRepository(
            HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DoctorLeave leave)
        {
            await _context.DoctorLeaves.AddAsync(leave);
        }

        public async Task<List<DoctorLeave>> GetByDoctorIdAsync(
            int doctorId)
        {
            return await _context.DoctorLeaves
                .Where(x => x.DoctorId == doctorId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.DoctorLeaves
                .AnyAsync(x =>
                    x.DoctorId == doctorId &&
                    startDate <= x.EndDate &&
                    endDate >= x.StartDate);
        }

        public async Task<DoctorLeave?> GetActiveLeaveForDateAsync(
            int doctorId,
            DateTime date)
        {
            return await _context.DoctorLeaves
                .FirstOrDefaultAsync(x =>
                    x.DoctorId == doctorId &&
                    date.Date >= x.StartDate.Date &&
                    date.Date <= x.EndDate.Date);
        }

        public async Task<List<DoctorLeave>> GetExpiredLeavesAsync()
        {
            var today = DateTime.Today;

            return await _context.DoctorLeaves
                .Where(x => x.EndDate.Date < today)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
