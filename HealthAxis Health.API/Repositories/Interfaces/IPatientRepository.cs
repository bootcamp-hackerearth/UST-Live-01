using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        #region Methods

        Task<Patient?> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default);

        Task<Patient?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<Patient?> GetPatientWithHealthRecordsAsync(
            int patientId,
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<Patient>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default);

        #endregion
    }
}