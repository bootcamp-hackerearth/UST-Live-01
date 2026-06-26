using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IHealthRecordService
    {
        Task<HealthRecordDto> AddRecordAsync(HealthRecordDto dto);

        Task<List<HealthRecordDto>> GetAllRecordsAsync();

        Task<HealthRecordDto?> GetRecordByIdAsync(int id);

        Task<List<HealthRecordDto>?> GetHealthRecordsByDoctorAsync(int? doctorId, int? patientId);
    }
}
