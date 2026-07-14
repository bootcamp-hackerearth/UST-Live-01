using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class DoctorLeaveRepository : IDoctorLeaveRepository
    {
        private readonly HealthAppDbContext _context;

        public DoctorLeaveRepository(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorLeave> AddAsync(DoctorLeave leave)
        {
            await _context.DoctorLeaves.AddAsync(leave);
            await _context.SaveChangesAsync();

            return leave;
        }

        public async Task<List<DoctorLeave>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.DoctorLeaves
                .Include(x => x.Doctor)
                .Where(x => x.DoctorId == doctorId)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync();
        }

        public async Task<(List<DoctorLeave> Items, int TotalCount)> GetAllLeaveDoctorAsync(int pageNumber,int pageSize)
        {
            var query = _context.DoctorLeaves
                .Include(l => l.Doctor)
                .OrderByDescending(l => l.CreatedDate)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> HasOverlappingLeaveAsync( int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.DoctorLeaves.AnyAsync(x =>
                x.DoctorId == doctorId &&
                x.StartDate.Date <= endDate.Date &&
                x.EndDate.Date >= startDate.Date);
        }

        public async Task<bool> IsDoctorOnLeaveAsync(int doctorId,
            DateTime date)
        {
            var selectedDate = date.Date;

            return await _context.DoctorLeaves.AnyAsync(x =>
                x.DoctorId == doctorId &&
                x.StartDate.Date <= selectedDate &&
                x.EndDate.Date >= selectedDate);
        }
    }
}