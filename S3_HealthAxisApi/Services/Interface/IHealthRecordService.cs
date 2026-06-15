using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IHealthRecordService
    {
        Task<HealthRecord?> GetByIdAsync(int id);

        Task<HealthRecord?> GetByAppointmentIdAsync(int appointmentId);

        Task AddHealthRecordAsync(HealthRecord record);

        Task UpdateHealthRecordAsync(int id, HealthRecord record);
    }
}
