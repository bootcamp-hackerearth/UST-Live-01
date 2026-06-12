using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Services.Interfaces
{
    public interface IHealthRecordService
    {
        List<HealthRecord> GetByPatient(int patientId);
        HealthRecord Create(HealthRecord record);
    }
}