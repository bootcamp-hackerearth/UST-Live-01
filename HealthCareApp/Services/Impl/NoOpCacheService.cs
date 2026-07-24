using HealthCareApp.Services.Interface;

namespace HealthCareApp.Services.Impl
{
    public class NoOpCacheService : ICacheService
    {
        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask;
        }
    }
}