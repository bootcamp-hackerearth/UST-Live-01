using HealthAxis.Shared.DTOs.HealthRecord;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordResponseDTO>> GetAllAsync();

        Task<HealthRecordResponseDTO?> GetByIdAsync(int id);

        Task<HealthRecordResponseDTO> CreateAsync(CreateHealthRecordDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<HealthRecordResponseDTO>> GetByPatientAsync(int patientId);
    }
}