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

        public async Task<HealthRecord?> GetHealthRecordsByDoctorAndPatientAsync(int? doctorId, int? patientId)
        {
            if (doctorId.HasValue && patientId.HasValue)
            {
                return await _context.Set<HealthRecord>().FirstAsync
                    (d => d.DoctorId == doctorId.Value && d.PatientId == patientId.Value);
            }
            if(doctorId.HasValue)
            {
                return await _context.Set<HealthRecord>().FirstAsync
                    (d => d.DoctorId == doctorId.Value);
            }
            if(patientId.HasValue)
            {
                return await _context.Set<HealthRecord>().FirstAsync
                    (d => d.PatientId == patientId.Value);
            }
            return null;
        }

    }
}
