using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Extensions;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
using System.Security.Claims;
using System.Text.Json;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class DoctorService(
        IDoctorRepository repository,
        IMapper mapper,
        IDistributedCache distributedCache
    ) : IDoctorService
    {
        private static readonly Serilog.ILogger Logger =
            Log.ForContext<DoctorService>();

        private const string DoctorIdClaimMissingMessage = "DoctorId claim missing";

        private const string DoctorNotFoundMessage = "Doctor not found";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<DoctorDto>> GetDoctorsAsync(
            string? specialisation,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException(DoctorIdClaimMissingMessage);

                var doctor = await repository.GetByIdAsync(doctorId, ct)
                    ?? throw new NotFoundException(DoctorNotFoundMessage);

                return new List<DoctorDto>
                {
                    mapper.Map<DoctorDto>(doctor)
                };
            }

            return mapper.Map<List<DoctorDto>>(
                await repository.GetDoctorsAsync(specialisation, ct));
        }

        public async Task<PagedResultDto<DoctorDto>> GetPagedAsync(
            string? specialisation,
            ClaimsPrincipal user,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException(DoctorIdClaimMissingMessage);

                var doctor = await repository.GetByIdAsync(doctorId, ct)
                    ?? throw new NotFoundException(DoctorNotFoundMessage);

                return new PagedResultDto<DoctorDto>
                {
                    Items = new List<DoctorDto>
                    {
                        mapper.Map<DoctorDto>(doctor)
                    },
                    PageNumber = 1,
                    PageSize = 1,
                    TotalCount = 1,
                    TotalPages = 1
                };
            }

            var totalCount = await repository.CountDoctorsAsync(
                specialisation,
                ct);

            var doctors = await repository.GetPagedDoctorsAsync(
                specialisation,
                pageNumber,
                pageSize,
                ct);

            return new PagedResultDto<DoctorDto>
            {
                Items = mapper.Map<List<DoctorDto>>(doctors),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<DoctorDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            if (user.IsDoctor())
            {
                var doctorId = user.GetDoctorId()
                    ?? throw new UnauthorizedException(DoctorIdClaimMissingMessage);

                if (doctorId != id)
                {
                    throw new UnauthorizedException(
                        "You can view only your own doctor profile");
                }
            }

            var doctor = await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(DoctorNotFoundMessage);

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<List<string>> GetAvailabilityAsync(
            int id,
            DateTime date,
            CancellationToken ct = default)
        {
            if (date.Date < DateTime.UtcNow.Date)
            {
                throw new InvalidException("Cannot check past date");
            }

            var cacheKey = BuildDoctorAvailabilityCacheKey(id, date);

            var cachedValue = await distributedCache.GetStringAsync(
                cacheKey,
                ct);

            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                var cachedSlots = JsonSerializer.Deserialize<List<string>>(
                    cachedValue,
                    JsonOptions);

                Logger.Information(
                    "[CACHE-HIT] Doctor availability served from Garnet | DoctorId={DoctorId} | Date={Date} | Key={CacheKey}",
                    id,
                    date.Date.ToString("yyyy-MM-dd"),
                    cacheKey);

                return cachedSlots ?? new List<string>();
            }

            Logger.Information(
                "[CACHE-MISS] Doctor availability not found in Garnet | DoctorId={DoctorId} | Date={Date} | Key={CacheKey}",
                id,
                date.Date.ToString("yyyy-MM-dd"),
                cacheKey);

            var availableSlots = await repository.GetAvailableSlotsAsync(
                id,
                date,
                ct);

            var serializedSlots = JsonSerializer.Serialize(
                availableSlots,
                JsonOptions);

            await distributedCache.SetStringAsync(
                cacheKey,
                serializedSlots,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                ct);

            Logger.Information(
                "[CACHE-SET] Doctor availability cached | DoctorId={DoctorId} | Date={Date} | Key={CacheKey} | TTL={TtlMinutes} minutes",
                id,
                date.Date.ToString("yyyy-MM-dd"),
                cacheKey,
                5);

            return availableSlots;
        }

        public async Task<DoctorDto> UpdateOwnStatusAsync(
            int id,
            bool isActive,
            ClaimsPrincipal user,
            CancellationToken ct = default)
        {
            var loggedInDoctorId = user.GetDoctorId()
                ?? throw new UnauthorizedException(DoctorIdClaimMissingMessage);

            if (loggedInDoctorId != id)
            {
                throw new UnauthorizedException(
                    "You can update only your own doctor profile status");
            }

            var doctor = await repository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException(DoctorNotFoundMessage);

            doctor.IsActive = isActive;

            var updated = await repository.UpdateAsync(
                id,
                doctor,
                ct)
                ?? throw new NotFoundException(DoctorNotFoundMessage);

            return mapper.Map<DoctorDto>(updated);
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
        }
    }
}