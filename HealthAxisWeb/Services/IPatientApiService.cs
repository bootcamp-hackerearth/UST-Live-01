using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IPatientApiService
    {
        Task<ApiResponseDto> Register(PatientDto dto);
        Task<PatientDto> GetById(int id);
        Task<ApiResponseDto> Update(int id, PatientDto dto);

        Task<List<HealthRecordDto>> GetHealthRecords(int patientId);
    }
}