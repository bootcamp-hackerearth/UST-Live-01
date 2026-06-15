using S3_HealthAxisApi.Enums;
using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByRoleAsync(UserRole role);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
    }
}