using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Api.Services
{
    public interface IHealthRecordService
    {
        ApiResponseDto Create(CreateHealthRecordDto dto);
        List<HealthRecordDto> GetByPatient(int patientId);

    }
}