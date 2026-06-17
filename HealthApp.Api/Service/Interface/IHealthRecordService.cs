using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IHealthRecordService
    {
        Task<HealthRecordDto> AddRecordAsync(HealthRecordDto dto);

        Task<List<HealthRecordDto>> GetAllRecordsAsync();

        Task<List<HealthRecordDto>?> GetHealthRecordsByDoctorAsync(int? doctorId, int? patientId);
    }
}
