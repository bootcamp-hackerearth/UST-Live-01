using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IHealthRecordRepository :
        IRepository<HealthRecord>
    {
        #region Methods

        Task<IEnumerable<HealthRecord>> GetByPatientIdAsync(
            int patientId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<HealthRecord>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken cancellationToken = default);

        Task<HealthRecord?> GetHealthRecordWithDetailsAsync(
            int recordId,
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<HealthRecord>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default);

        Task<HealthRecord?> GetByAppointmentIdAsync(
            int appointmentId,
            CancellationToken cancellationToken = default);

        #endregion
    }
}