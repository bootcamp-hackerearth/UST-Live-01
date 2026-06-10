using HealthCare.Shared;
using HealthCare.Shared.DTOs.HealthRecord;
using System.Threading.Tasks;

namespace HealthCare.Web.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<PagedResult<HealthRecordDto>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber,
            int pageSize);

     
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateHealthRecordDto dto);
    }
}