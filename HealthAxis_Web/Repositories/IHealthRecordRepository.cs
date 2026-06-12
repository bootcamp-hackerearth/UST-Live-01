using HealthAxis.Api.Models;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetByPatientId(int patientId);
    }
}