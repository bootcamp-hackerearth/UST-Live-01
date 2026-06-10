using HealthAxis.Shared.Dtos;
using System.Collections.Generic;

namespace HealthAxis.Api.Services
{
    public interface IHealthRecordService
    {
        ApiResponseDto Add(HealthRecordDto dto);

        List<HealthRecordDto> GetByPatient(int patientId);

        HealthRecordDto GetById(int recordId);
    }
}