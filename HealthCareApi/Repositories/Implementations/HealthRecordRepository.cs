//using HealthCareApi.Helper;
using HealthCare.Shared;
using HealthCareApi;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using HealthCareWebApi;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCareApi.Repositories.Implementations
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAppDbContext context) : base(context)
        {
        }

        // Check duplicate record
        public async Task<bool> HealthRecordExistsAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .AnyAsync(h => h.AppointmentId == appointmentId);
        }

        // Use VIEW instead of joins
        public async Task<PagedResult<vw_PatientHealthHistory>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber,
            int pageSize)
        {
            //  Safety
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize > 50 ? 50 : pageSize;

            //  Base query
            var query = _context.vw_PatientHealthHistory
                .Where(v => v.PatientId == patientId);

            //   IMPORTANT: total count BEFORE pagination
            int totalCount = await query.CountAsync();

            //  Sorting
            query = query.OrderByDescending(v => v.VisitDate);

            //  Pagination
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //  Return paged result
            return new PagedResult<vw_PatientHealthHistory>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<HealthRecord> GetByAppointmentIdAsync(int appointmentId)
        {
            return await _context.HealthRecords
                .FirstOrDefaultAsync(h => h.AppointmentId == appointmentId);
        }
    }
} 