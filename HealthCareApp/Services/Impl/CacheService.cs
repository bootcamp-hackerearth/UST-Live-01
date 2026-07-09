using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using HealthCareApp.Services.Interface;

namespace HealthCareApp.Services.Impl
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache cache;

        public CacheService(
            IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedData =
                await cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiry)
        {
            var json =
                JsonSerializer.Serialize(value);

            await cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                });
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }
    }
}