using HealthAxis.API.Data;
using Microsoft.EntityFrameworkCore;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace S3_HealthAxisApi.Repository.Implementation
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : IUserRepository
    {
        private readonly HealthAxisDbContext _context;

        public UserRepository(HealthAxisDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _context.AppUsers
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.AppUsers.AddAsync(user);
        }

        public Task UpdateAsync(User user)
        {
            _context.AppUsers.Update(user);
            return Task.CompletedTask;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.AppUsers
                .AnyAsync(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}