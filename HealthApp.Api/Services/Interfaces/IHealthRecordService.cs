using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto?> GetByIdAsync(int id);
        Task<HealthRecordDto> CreateAsync(HealthRecordCreateDto dto);
    }
}
