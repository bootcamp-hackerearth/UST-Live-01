using HealthAxisCore_Api.Models.DTOs;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task<HealthRecordDto> AddAsync(HealthRecordDto entity);
        Task<HealthRecordDto> UpdateAsync(int id, HealthRecordDto entity);
    }
}
