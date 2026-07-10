using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Repository.Impl
{
    public class DoctorLeaveRepository : Repository<DoctorLeave>, IDoctorLeaveRepository
    {
        private readonly DbContext dbContext;

        public DoctorLeaveRepository(DbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<DoctorLeave>> GetLeavesByDoctorIdAsync(int doctorId)
        {
            return await dbContext.Set<DoctorLeave>()
                .Include(doctorLeave => doctorLeave.Doctor)
                .Where(doctorLeave => doctorLeave.DoctorId == doctorId)
                .OrderByDescending(doctorLeave => doctorLeave.StartDate)
                .ToListAsync();
        }

        public async Task<bool> IsDoctorOnLeaveAsync(int doctorId, DateTime date)
        {
            var selectedDate = date.Date;

            return await dbContext.Set<DoctorLeave>()
                .AnyAsync(doctorLeave =>
                    doctorLeave.DoctorId == doctorId &&
                    doctorLeave.StartDate.Date <= selectedDate &&
                    doctorLeave.EndDate.Date >= selectedDate);
        }

        public async Task<bool> HasOverlappingLeaveAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            var leaveStartDate = startDate.Date;

            var leaveEndDate = endDate.Date;

            return await dbContext.Set<DoctorLeave>()
                .AnyAsync(doctorLeave =>
                    doctorLeave.DoctorId == doctorId &&
                    doctorLeave.StartDate.Date <= leaveEndDate &&
                    doctorLeave.EndDate.Date >= leaveStartDate);
        }
    }
}