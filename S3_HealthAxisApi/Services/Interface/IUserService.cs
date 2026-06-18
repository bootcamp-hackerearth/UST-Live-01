using S3_HealthAxisApi.Models;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByRefreshTokenAsync(string refreshToken);

        Task<bool> EmailExistsAsync(string email);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task SaveChangesAsync();
    }
}