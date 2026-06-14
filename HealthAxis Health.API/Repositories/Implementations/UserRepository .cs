using HealthAxisHealth.API.Data;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisHealth.API.Repositories.Implementations
{
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
            string email)
        {
            return await _context.Users
                .Include(u => u.Patient)
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(
                    u => u.Email == email);
        }

        public async Task<User?> GetByRefreshTokenAsync(
            string refreshToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(
                    u => u.RefreshToken == refreshToken);
        }

        public async Task<PagedResultDto<User>> GetPagedAsync(
            PaginationParams pagination,
            UserRole? role = null)
        {
            IQueryable<User> query =
                _context.Users
                    .Include(u => u.Patient)
                    .Include(u => u.Doctor);

            // Filter by role
            if (role.HasValue)
            {
                query = query.Where(
                    u => u.Role == role.Value);
            }

            // Search by email or full name
            if (!string.IsNullOrWhiteSpace(
                pagination.Search))
            {
                string search =
                    pagination.Search
                        .Trim()
                        .ToLower();

                query = query.Where(u =>
                    u.Email.ToLower().Contains(search)
                    ||
                    (u.Doctor != null &&
                     u.Doctor.FullName
                        .ToLower()
                        .Contains(search))
                    ||
                    (u.Patient != null &&
                     u.Patient.FullName
                        .ToLower()
                        .Contains(search)));
            }

            int totalRecords =
                await query.CountAsync();

            List<User> users =
                await query
                    .OrderBy(u => u.Email)
                    .Skip(
                        (pagination.PageNumber - 1)
                        * pagination.PageSize)
                    .Take(
                        pagination.PageSize)
                    .ToListAsync();

            return new PagedResultDto<User>
            {
                Items = users,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalRecords = totalRecords
            };
        }

        #endregion
    }
}
