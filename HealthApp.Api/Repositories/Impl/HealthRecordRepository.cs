using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl
{
    public class HealthRecordRepository(HealthAppDbContext context) : Repository<HealthRecord>(context), IHealthRecordRepository
    {

        public async Task<IEnumerable<HealthRecord>> GetHealthRecordsAsync(
            int? patientId = null,
            int? appointmentId = null)
        {
            var query = context.HealthRecords.AsQueryable();

            if (patientId.HasValue)
            {
                query = query.Where(r => r.PatientId == patientId.Value);
            }

            if (appointmentId.HasValue)
            {
                query = query.Where(r => r.AppointmentId == appointmentId.Value);
            }

            return await query.ToListAsync();
        }

    }
}
