using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAxis.Web.Services
{
    public interface IPatientApiService
    {
        Task<ApiResponseDto> Register(PatientDto dto);

        Task<List<PatientDto>> GetAll();

        Task<PatientDto> GetById(int id);

        Task<ApiResponseDto> Update(int id, PatientDto dto);

        Task<ApiResponseDto> Deactivate(int id);

        Task<List<HealthRecordDto>> GetHealthRecords(int patientId);
    }
}