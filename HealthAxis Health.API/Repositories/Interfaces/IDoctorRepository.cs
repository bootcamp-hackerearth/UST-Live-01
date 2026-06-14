using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IDoctorRepository :
        IRepository<Doctor>
    {
        #region Methods

        Task<IEnumerable<Doctor>>
            GetActiveDoctorsAsync();

        Task<IEnumerable<Doctor>>
            GetBySpecialisationAsync(
                Specialisation specialisation);

        Task<Doctor?>
            GetDoctorWithAppointmentsAsync(
                int doctorId);

        Task<Doctor?>
            GetByUserIdAsync(
                int userId);

        Task<PagedResultDto<Doctor>>
            GetPagedAsync(
                PaginationParams pagination);

        #endregion
    }
}
