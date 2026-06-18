using HealthAxis.API.Data;
using HealthAxis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Repositories
{
    public class HealthRecordRepository
        : Repository<HealthRecord>, IHealthRecordRepository
    {
        public HealthRecordRepository(HealthAxisDbContext context)
            : base(context)
        {
        }

        public async Task<List<HealthRecord>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(record => record.PatientId == patientId)
                .ToListAsync(ct);
        }
    }
}
