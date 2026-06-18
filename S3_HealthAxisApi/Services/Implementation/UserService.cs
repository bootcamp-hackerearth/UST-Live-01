using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetByEmailAsync(
            string email)
        {
            return await _userRepository
                .GetByEmailAsync(email);
        }

        public async Task<User?> GetByRefreshTokenAsync(
            string refreshToken)
        {
            return await _userRepository
                .GetByRefreshTokenAsync(refreshToken);
        }

        public async Task<bool> EmailExistsAsync(
            string email)
        {
            return await _userRepository
                .EmailExistsAsync(email);
        }

        public async Task CreateAsync(
            User user)
        {
            await _userRepository
                .AddAsync(user);
        }

        public async Task UpdateAsync(
            User user)
        {
            await _userRepository
                .UpdateAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await _userRepository
                .SaveChangesAsync();
        }
    }
}