using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IHealthRecordApiService
    {
        Task<ApiResponseDto> Create(CreateHealthRecordDto dto);
        Task<List<HealthRecordDto>> GetByPatient(int patientId);

    }
}