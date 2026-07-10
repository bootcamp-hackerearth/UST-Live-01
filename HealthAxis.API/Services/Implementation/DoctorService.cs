using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.Enums;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthAxis.API.Services.Implementation
{
    public class DoctorService(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper,
        IDistributedCache distributedCache,
        ILogger<DoctorService> logger) : IDoctorService
    {
        private const int CacheExpiryMinutes = 5;

        private static readonly string[] HospitalTimeSlots =
        [
            "09:00 AM - 10:00 AM",
            "10:00 AM - 11:00 AM",
            "11:00 AM - 12:00 PM",
            "12:00 PM - 01:00 PM",
            "02:00 PM - 03:00 PM",
            "03:00 PM - 04:00 PM",
            "04:00 PM - 05:00 PM",
            "05:00 PM - 06:00 PM",
            "06:00 PM - 07:00 PM",
            "07:00 PM - 08:00 PM",
            "08:00 PM - 09:00 PM",
            "09:00 PM - 10:00 PM"
        ];

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<DoctorDto>> GetAllAsync()
        {
            var doctors = await doctorRepository.GetAllAsync();

            return mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetByIdAsync(int id)
        {
            var doctor = await doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> GetByUserIdAsync(string userId)
        {
            var doctors = await doctorRepository.GetAllAsync();

            var doctor = doctors.FirstOrDefault(item => item.UserId == userId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor profile not found");
            }

            return mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorAvailabilityDto> GetAvailabilityAsync(
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
                var cachedAvailability =
                    JsonSerializer.Deserialize<DoctorAvailabilityDto>(
                        cachedValue,
                        JsonOptions);

                if (cachedAvailability != null)
                {
                    LogCacheHit(
                        id,
                        availabilityDate,
                        cacheKey,
                        cachedAvailability.AvailableSlots.Count);

                    return cachedAvailability;
                }
            }

            LogCacheMiss(
                id,
                availabilityDate,
                cacheKey);

            var doctor = await doctorRepository.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            var availableSlots = await GetAvailableSlotsAsync(
                id,
                availabilityDate,
                doctor.IsActive);

            var availability = new DoctorAvailabilityDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                IsActive = doctor.IsActive,
                Date = availabilityDate,
                Message = doctor.IsActive
                    ? "Doctor is available"
                    : "Doctor is not available",
                AvailableSlots = availableSlots
            };

            var serializedAvailability = JsonSerializer.Serialize(
                availability,
                JsonOptions);

            await distributedCache.SetStringAsync(
                cacheKey,
                serializedAvailability,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(CacheExpiryMinutes)
                });

            LogCacheStored(
                id,
                availabilityDate,
                cacheKey,
                availableSlots.Count);

            return availability;
        }

        private async Task<List<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateTime availabilityDate,
            bool isDoctorActive)
        {
            if (!isDoctorActive)
            {
                return new List<string>();
            }

            var appointments = await appointmentRepository.GetAllAsync();

            var bookedSlots = appointments
                .Where(appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.ScheduledDate.Date == availabilityDate &&
                    IsActiveAppointmentStatus(appointment.Status))
                .Select(appointment => NormalizeTimeSlot(appointment.TimeSlot))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return HospitalTimeSlots
                .Where(slot => !bookedSlots.Contains(NormalizeTimeSlot(slot)))
                .ToList();
        }

        private static bool IsActiveAppointmentStatus(
            AppointmentStatus status)
        {
            return status == AppointmentStatus.Pending ||
                   status == AppointmentStatus.Confirmed;
        }

        private static string NormalizeTimeSlot(string timeSlot)
        {
            return timeSlot.Trim();
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
        }

        private void LogCacheMiss(
            int doctorId,
            DateTime date,
            string cacheKey)
        {
            logger.LogInformation(
                """
                ========================================
                CACHE MISS - LOADING FROM DATABASE
                ========================================
                Doctor Id : {DoctorId}
                Date      : {Date:yyyy-MM-dd}
                Key       : {CacheKey}
                Source    : SQL Server
                ========================================
                """,
                doctorId,
                date,
                cacheKey);
        }

        private void LogCacheStored(
            int doctorId,
            DateTime date,
            string cacheKey,
            int availableSlotCount)
        {
            logger.LogInformation(
                """
                ========================================
                CACHE STORED IN GARNET
                ========================================
                Doctor Id       : {DoctorId}
                Date            : {Date:yyyy-MM-dd}
                Key             : {CacheKey}
                TTL             : {CacheExpiryMinutes} Minutes
                Available Slots : {AvailableSlotCount}
                ========================================
                """,
                doctorId,
                date,
                cacheKey,
                CacheExpiryMinutes,
                availableSlotCount);
        }

        private void LogCacheHit(
            int doctorId,
            DateTime date,
            string cacheKey,
            int availableSlotCount)
        {
            logger.LogInformation(
                """
                ========================================
                CACHE HIT - LOADING FROM GARNET
                ========================================
                Doctor Id       : {DoctorId}
                Date            : {Date:yyyy-MM-dd}
                Key             : {CacheKey}
                Source          : Garnet
                Available Slots : {AvailableSlotCount}
                ========================================
                """,
                doctorId,
                date,
                cacheKey,
                availableSlotCount);
        }
    }
}