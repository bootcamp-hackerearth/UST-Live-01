using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>> GetAllAsync(CancellationToken ct = default);
        Task<HealthRecordDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<HealthRecordDto> CreateAsync(HealthRecordCreateDto dto, CancellationToken ct = default);
    }
}
