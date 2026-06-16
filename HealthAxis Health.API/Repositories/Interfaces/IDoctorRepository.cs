using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IDoctorRepository :
        IRepository<Doctor>
    {
        #region Methods

        Task<IEnumerable<Doctor>> GetActiveDoctorsAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Doctor>> GetBySpecialisationAsync(
            Specialisation specialisation,
            CancellationToken cancellationToken = default);

        Task<Doctor?> GetDoctorWithAppointmentsAsync(
            int doctorId,
            CancellationToken cancellationToken = default);

        Task<Doctor?> GetByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<Doctor>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default);

        #endregion
    }
}