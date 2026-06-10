using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IHealthRecordApiService
    {
        Task<ApiResponseDto> AddHealthRecord(HealthRecordDto dto);

        Task<List<HealthRecordDto>> GetByPatient(int patientId);

        Task<HealthRecordDto> GetById(int recordId);
    }
}