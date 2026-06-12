using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Services.Interfaces;

namespace Healthaxis2.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly AppDbContext db;

        public HealthRecordService(AppDbContext context)
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

        public HealthRecord Create(HealthRecord record)
        {
            db.HealthRecords.Add(record);
            db.SaveChanges();
            return record;
        }
    }
}