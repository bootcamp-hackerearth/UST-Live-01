using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.DoctorDtos;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthAxis.API.Services.Implementation
{
    public class DoctorService(
        IDoctorRepository repository,
        IMapper mapper,
        IDistributedCache distributedCache,
        ILogger<DoctorService> logger) : IDoctorService
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            return mapper.Map<List<DoctorDto>>(
                await repository.GetAllAsync());
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> GetByUserIdAsync(string userId)
        {
            var doctors = await repository.GetAllAsync();

            var doctor = doctors.FirstOrDefault(item => item.UserId == userId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor profile not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto> GetAvailabilityAsync(
            int id,
            DateTime? date = null)
        {
            var availabilityDate = (date ?? DateTime.Today).Date;

            if (availabilityDate < DateTime.Today)
            {
                throw new ValidationExceptions(
                    "Cannot check doctor availability for a past date.");
            }

            var cacheKey = BuildDoctorAvailabilityCacheKey(
                id,
                availabilityDate);

            var cachedValue = await distributedCache.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                var cachedDoctor = JsonSerializer.Deserialize<DoctorDto>(
                    cachedValue,
                    JsonOptions);

                logger.LogInformation(
                    "Garnet cache HIT for doctor availability. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                    id,
                    availabilityDate,
                    cacheKey);

                if (cachedDoctor != null)
                {
                    return cachedDoctor;
                }
            }

            logger.LogInformation(
                "Garnet cache MISS for doctor availability. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                id,
                availabilityDate,
                cacheKey);

            var doctor = await repository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            var doctorDto = mapper.Map<DoctorDto>(doctor);

            var serializedDoctor = JsonSerializer.Serialize(
                doctorDto,
                JsonOptions);

            await distributedCache.SetStringAsync(
                cacheKey,
                serializedDoctor,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            logger.LogInformation(
                "Doctor availability stored in Garnet cache. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}, TtlMinutes: {TtlMinutes}",
                id,
                availabilityDate,
                cacheKey,
                5);

            return doctorDto;
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
        }
    }
}