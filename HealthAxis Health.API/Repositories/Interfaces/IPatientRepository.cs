using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IPatientRepository :
        IRepository<Patient>
    {
        #region Methods

        Task<Patient?> GetByUserIdAsync(
            int userId);

        Task<Patient?> GetByEmailAsync(
            string email);

        Task<Patient?> GetPatientWithHealthRecordsAsync(
            int patientId);

        Task<PagedResultDto<Patient>>
            GetPagedAsync(
                PaginationParams pagination);

        #endregion
    }
}
