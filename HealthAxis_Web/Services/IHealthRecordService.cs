using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public interface IHealthRecordService
    {
        List<HealthRecordDto> GetByPatient(int patientId);
    }
}