using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task<HealthRecordDto> AddAsync(HealthRecordDto entity);
        Task<HealthRecordDto> UpdateAsync(int id, HealthRecordDto entity);
    }
}
