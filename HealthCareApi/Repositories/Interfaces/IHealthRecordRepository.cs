using HealthCare.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCareApi.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
        Task<bool> HealthRecordExistsAsync(int appointmentId);

        Task<PagedResult<vw_PatientHealthHistory>> GetPatientHealthHistoryAsync(
             int patientId,
             int pageNumber,
             int pageSize);
        Task<HealthRecord> GetByAppointmentIdAsync(int appointmentId);
    }
}