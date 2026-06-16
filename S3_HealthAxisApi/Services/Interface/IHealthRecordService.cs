using S3_HealthAxisApi.DTOs.HealthRecord;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IHealthRecordService
    {
        Task<HealthRecordDto?> GetByIdAsync(int id);

        Task<HealthRecordDto?> GetByAppointmentIdAsync(int appointmentId);

        Task<HealthRecordDto> CreateAsync(CreateHealthRecordDto dto);

        Task UpdateAsync(int id, UpdateHealthRecordDto dto);
    }
}