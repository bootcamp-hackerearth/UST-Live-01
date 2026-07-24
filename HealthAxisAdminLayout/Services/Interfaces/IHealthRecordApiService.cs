using HealthAxis.Shared.DTOs.HealthRecord;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IHealthRecordApiService
    {
        Task<List<HealthRecordResponseDto>> GetHealthRecordsAsync();

        Task<bool> DeleteHealthRecordAsync(int id);
    }
}

