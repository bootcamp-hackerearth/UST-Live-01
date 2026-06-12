using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Repositories.Interfaces
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetByPatient(int patientId);
        void Add(HealthRecord record);
    }
}