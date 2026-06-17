using HealthAxisApplicn.Data;
using HealthAxisApplicn.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Repositories.Impl
{
    public class HealthRecordRepository : Repository<HealthRecord>, IHealthRecordRepository
    {
        private readonly AppDbContext _context;
        public HealthRecordRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<HealthRecord>> GetRecordByPatientIDAsync(int patientId, CancellationToken ct = default)
        {
            var recordsByPatientID = await _context.Set<HealthRecord>().Where(h => h.PatientId == patientId).ToListAsync(ct);
            return recordsByPatientID;
        }

        public async Task<List<HealthRecord>> GetRecordsByDoctorIDAsync(int doctorId, CancellationToken ct = default)
        {
            var recordsByDoctorID = await _context.Set<HealthRecord>().Where(h => h.DoctorId == doctorId).ToListAsync(ct);
            return recordsByDoctorID;
        }

        public async Task<List<HealthRecord>> GetRecordsByDoctorNameAsync(string doctorName, CancellationToken ct = default)
        {
            var recordsByDoctor = await _context.Set<HealthRecord>().Where(h => h.Doctor.DoctorName == doctorName).ToListAsync(ct);
            return recordsByDoctor;
        }

        public async Task<List<HealthRecord>> GetRecordsByPatientNameAsync(string patientName, CancellationToken ct = default)
        {
            var recordsByPatient = await  _context.Set<HealthRecord>().Where(h => h.Patient.PatientName == patientName).ToListAsync(ct);
            return recordsByPatient;
        }
    }
}
