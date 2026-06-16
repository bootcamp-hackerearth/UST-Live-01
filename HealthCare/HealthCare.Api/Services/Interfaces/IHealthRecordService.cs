using HealthCare.Api.Models;
using HealthCare.Api.DTOs.HealthRecord;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task AddAsync(CreateHealthRecordDto dto);
        Task UpdateAsync(int id, UpdateHealthRecordDto dto);
        Task DeleteAsync(int  id);
        Task<HealthRecordListDto> GetByIdAsync(int id);
        Task<IEnumerable<HealthRecordListDto>> GetAllAsync();
    }
}
