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

        Task<bool> CreateAsync(CreateHealthRecordDto dto);
        Task<HealthRecordDto> GetByAppointmentIdAsync(int appointmentId);
        Task<HealthRecordDto> GetByIdAsync(int recordId);
    }
}