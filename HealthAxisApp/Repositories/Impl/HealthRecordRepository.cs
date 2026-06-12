using HealthAxisApp.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace HealthAxisApp.Repositories.Impl
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly HealthAxisEntities2 _context;

        public HealthRecordRepository(HealthAxisEntities2 context)
        {
            _context = context;
        }

        public IEnumerable<HealthRecord> GetByPatient(int patientId)
        {
            return _context.HealthRecords
                .Include(r => r.Patient)
                .Include(r => r.Doctor)
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public HealthRecord Add(HealthRecord record)
        {
            _context.HealthRecords.Add(record);
            _context.SaveChanges();

            return record;
        }
    }
}