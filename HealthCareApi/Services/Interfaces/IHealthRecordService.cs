using HealthCare.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareApi.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<HealthRecord> AddHealthRecordAsync(HealthRecord record);
        Task<PagedResult<vw_PatientHealthHistory>> GetPatientHealthHistoryAsync(
            int patientId,
            int pageNumber = 1,
            int pageSize = 10);
        Task<HealthRecord> GetByAppointmentIdAsync(int id);
        Task<HealthRecord> GetByIdAsync(int id);
    }
}