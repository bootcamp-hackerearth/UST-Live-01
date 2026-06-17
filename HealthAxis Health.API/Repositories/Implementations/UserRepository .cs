using HealthAxisHealth.API.Data;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Repositories.Implementations
{
    [ExcludeFromCodeCoverage]
    public class UserRepository :
        Repository<User>,
        IUserRepository
    {
        #region Constructor

        public UserRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        #endregion

        #region Methods

        public async Task<User?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(
                    u => u.Email == email,
                    cancellationToken);
        }

        public async Task<User?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.RefreshToken == refreshToken,
                    cancellationToken);
        }

        public async Task<PagedResultDto<User>> GetPagedAsync(
            PaginationParams pagination,
            UserRole? role = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor);

            if (role.HasValue)
            {
                query = query.Where(u => u.Role == role.Value);
            }

            if (!string.IsNullOrWhiteSpace(pagination.Search))
            {
                string search = pagination.Search.Trim();

                query = query.Where(u =>
                    u.Email.Contains(search) ||
                    (u.Doctor != null &&
                     u.Doctor.FullName.Contains(search)) ||
                    (u.Patient != null &&
                     u.Patient.FullName.Contains(search)));
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            int pageNumber = pagination.PageNumber ?? 1;
            int pageSize = pagination.PageSize ?? 10;

            List<User> users = await query
                .OrderBy(u => u.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultDto<User>
            {
                Items = users,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}