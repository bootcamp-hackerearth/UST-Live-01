using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.HealthRecord;
using HealthCare.Api.Models;

namespace HealthCare.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task AddAsync(CreateHealthRecordDto dto);
        Task UpdateAsync(int id, UpdateHealthRecordDto dto);
        Task DeleteAsync(int  id);
        Task<HealthRecordListDto> GetByIdAsync(int id);
        Task<PagedResult<HealthRecordListDto>> GetAllAsync(HealthRecordFilter filter);

        Task<List<HealthRecordListDto>> GetHealthRecordByPatient(int id);
        Task<List<HealthRecordListDto>> GetHealthRecordByAppointment(int id);


    }
}
