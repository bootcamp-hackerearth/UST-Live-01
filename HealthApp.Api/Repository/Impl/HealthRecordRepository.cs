using HealthApp.Api.Data;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repository.Impl
{
    public class HealthRecordRepository : GenericRepository<HealthRecord>, IHealthRecordRepository
    {
        private readonly HealthAppDbContext _context;
        public HealthRecordRepository(HealthAppDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task<List<HealthRecord?>> GetHealthRecordsByDoctorAndPatientAsync(int? doctorId, int? patientId)
        {
            var query = _context.Set<HealthRecord>().AsQueryable();

            if (doctorId.HasValue)
            {
                query = query.Where(d => d.DoctorId == doctorId.Value);
            }

            if (patientId.HasValue)
            {
                query = query.Where(d => d.PatientId == patientId.Value);
            }

            return await query.ToListAsync();
        }

       
    }
}
