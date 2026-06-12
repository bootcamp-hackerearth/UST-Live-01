using HealthAxisApp.Data;
using System.Collections.Generic;

namespace HealthAxisApp.Repositories
{
    public interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetByPatient(int patientId);

        HealthRecord Add(HealthRecord record);
    }
}