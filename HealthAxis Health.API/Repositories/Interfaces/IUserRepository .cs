using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        #region Methods

        Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<User?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<User>> GetPagedAsync(
            PaginationParams pagination,
            UserRole? role = null,
            CancellationToken cancellationToken = default);

        #endregion
    }
}