using HealthCareApp.Services.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthCareApp.Services.Impl
{
    public class CacheService : ICacheService
    {
        private static readonly JsonSerializerOptions SerializerOptions =
            new()
            {
                PropertyNameCaseInsensitive = true
            };

        private readonly IDistributedCache distributedCache;

        private readonly ILogger<CacheService> logger;

        public CacheService(
            IDistributedCache distributedCache,
            ILogger<CacheService> logger)
        {
            this.distributedCache = distributedCache;
            this.logger = logger;
        }

        public async Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            var cachedValue = await distributedCache.GetStringAsync(
                key,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                LogCacheMiss<T>(key);

                return default;
            }

            try
            {
                var deserializedValue = JsonSerializer.Deserialize<T>(
                    cachedValue,
                    SerializerOptions);

                LogCacheHit<T>(key);

                return deserializedValue;
            }
            catch (JsonException ex)
            {
                LogCacheDeserializationWarning<T>(
                    ex,
                    key);

                await RemoveAsync(
                    key,
                    cancellationToken);

                return default;
            }
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan absoluteExpirationRelativeToNow,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            if (absoluteExpirationRelativeToNow <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(absoluteExpirationRelativeToNow),
                    "Cache expiration must be greater than zero.");
            }

            var serializedValue = JsonSerializer.Serialize(
                value,
                SerializerOptions);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    absoluteExpirationRelativeToNow
            };

            await distributedCache.SetStringAsync(
                key,
                serializedValue,
                cacheOptions,
                cancellationToken);

            LogCacheStored<T>(
                key,
                absoluteExpirationRelativeToNow);
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            await distributedCache.RemoveAsync(
                key,
                cancellationToken);

            LogCacheRemoved(key);
        }

        private void LogCacheMiss<T>(string key)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            logger.LogDebug(
                "Distributed cache miss. CacheKey: {CacheKey}, CacheValueType: {CacheValueType}",
                key,
                typeof(T).Name);
        }

        private void LogCacheHit<T>(string key)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            logger.LogDebug(
                "Distributed cache hit. CacheKey: {CacheKey}, CacheValueType: {CacheValueType}",
                key,
                typeof(T).Name);
        }

        private void LogCacheStored<T>(
            string key,
            TimeSpan absoluteExpirationRelativeToNow)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            logger.LogDebug(
                "Distributed cache entry stored. CacheKey: {CacheKey}, CacheValueType: {CacheValueType}, CacheDurationSeconds: {CacheDurationSeconds}",
                key,
                typeof(T).Name,
                absoluteExpirationRelativeToNow.TotalSeconds);
        }

        private void LogCacheRemoved(string key)
        {
            if (!logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            logger.LogDebug(
                "Distributed cache entry removed. CacheKey: {CacheKey}",
                key);
        }

        private void LogCacheDeserializationWarning<T>(
            JsonException exception,
            string key)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Failed to deserialize distributed cache value. CacheKey: {CacheKey}, CacheValueType: {CacheValueType}. The invalid cache entry will be removed.",
                key,
                typeof(T).Name);
        }
    }
}