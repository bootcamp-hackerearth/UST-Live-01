using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IHealthRecordService
        : IService<HealthRecord, HealthRecordReadDto, HealthRecordCreateDto, HealthRecordUpdateDto>
    {
        Task<List<HealthRecordReadDto>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);
    }
}
