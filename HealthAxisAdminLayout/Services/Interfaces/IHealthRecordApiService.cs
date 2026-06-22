using HealthAxisAdminLayout.DTOs.HealthRecord;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IHealthRecordApiService
    {
        Task<List<HealthRecordResponseDTO>> GetHealthRecordsAsync();

        Task<bool> DeleteHealthRecordAsync(int id);
    }
}