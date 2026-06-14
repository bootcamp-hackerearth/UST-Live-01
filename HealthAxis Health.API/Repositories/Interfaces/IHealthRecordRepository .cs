using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IHealthRecordRepository :
        IRepository<HealthRecord>
    {
        #region Methods

        Task<IEnumerable<HealthRecord>>
            GetByPatientIdAsync(
                int patientId);

        Task<IEnumerable<HealthRecord>>
            GetByDoctorIdAsync(
                int doctorId);

        Task<HealthRecord?>
            GetHealthRecordWithDetailsAsync(
                int recordId);

        Task<PagedResultDto<HealthRecord>>
            GetPagedAsync(
                PaginationParams pagination);
        Task<HealthRecord?> GetByAppointmentIdAsync(
            int appointmentId);

        #endregion
    }
}
