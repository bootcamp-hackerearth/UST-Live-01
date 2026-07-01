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
            var query = _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .AsQueryable();

            if (doctorId.HasValue)
            {
                query = query.Where(r => r.DoctorId == doctorId.Value);
            }

            if (patientId.HasValue)
            {
                query = query.Where(r => r.PatientId == patientId.Value);
            }

            return await query.ToListAsync();
        }


        public async Task<List<HealthRecord>?> getallAsync()
        {
            return await _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .ToListAsync();
        }
        public async Task<HealthRecord?> getbyidAsync(int id)
        {
            return await _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .FirstOrDefaultAsync(r => r.RecordId == id);
        }


        public async Task<List<HealthRecord>> GetByPatientIdAsync(int patientId)
        {
            return await _context.HealthRecords
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .ToListAsync();
        }

        public async Task<List<HealthRecord>> GetByDoctorIdAsync(int doctorId)
        {
            return await _context.HealthRecords
                .Where(r => r.DoctorId == doctorId)
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .ToListAsync();
        }

    }
}
