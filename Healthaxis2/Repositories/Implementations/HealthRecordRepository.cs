using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Repositories.Interfaces;

namespace Healthaxis2.Repositories.Implementations
{
    public class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly AppDbContext db;

        public HealthRecordRepository(AppDbContext context)
        {
            db = context;
        }

        public List<HealthRecord> GetByPatient(int patientId)
        {
            return db.HealthRecords
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.VisitDate)
                .ToList();
        }

        public void Add(HealthRecord record)
        {
            db.HealthRecords.Add(record);
            db.SaveChanges();
        }
    }
}