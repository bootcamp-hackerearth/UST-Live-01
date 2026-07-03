using HealthAxis.Shared.DTOs.HealthRecord;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordResponseDto>> GetAllAsync();

        Task<HealthRecordResponseDto?> GetByIdAsync(int id);

        Task<HealthRecordResponseDto> CreateAsync(CreateHealthRecordDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<HealthRecordResponseDto>> GetByPatientAsync(int patientId);
    }
}