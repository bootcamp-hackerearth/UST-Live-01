using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IUserRepository :
        IRepository<User>
    {
        #region Methods

        Task<User?>
            GetByEmailAsync(
                string email);

        Task<User?>
            GetByRefreshTokenAsync(
                string refreshToken);

        Task<PagedResultDto<User>>
            GetPagedAsync(
                PaginationParams pagination,
                UserRole? role = null);

        #endregion
    }
}
