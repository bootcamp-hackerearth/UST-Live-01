using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace HealthApp.API.Service.Impl;

public class DoctorService(
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    IHttpContextAccessor httpContextAccessor,
    IDistributedCache distributedCache,
    IMapper mapper,
    ILogger<DoctorService> logger) : IDoctorService
{
    private const string DoctorEntityName = "Doctor";

    public async Task<List<DoctorDto>> GetAllDoctorsAsync()
    {
        if (IsDoctor())
        {
            throw new ForbiddenAccessException(
                "Doctors are not allowed to search all doctors.");
        }

        return mapper.Map<List<DoctorDto>>(
            await doctorRepository.GetAllActiveAsync());
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(int doctorId)
    {
        ValidateDoctorId(doctorId);

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (loggedInDoctor.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's profile.");
            }
        }

        var doctor = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException(DoctorEntityName, doctorId);

        if (!doctor.IsActive)
        {
            throw new EntityNotFoundException(DoctorEntityName, doctorId);
        }

        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> GetLoggedInDoctorProfileAsync()
    {
        var doctor = await GetLoggedInDoctorAsync();

        if (!doctor.IsActive)
        {
            throw new ForbiddenAccessException(
                "Your doctor profile is inactive.");
        }

        return mapper.Map<DoctorDto>(doctor);
    }

    public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(
        SpecialisationType specialisation)
    {
        if (IsDoctor())
        {
            throw new ForbiddenAccessException(
                "Doctors are not allowed to search doctors by specialisation.");
        }

        if (!Enum.IsDefined(specialisation))
        {
            throw new BusinessRuleException("Invalid specialisation.");
        }

        var doctors = await doctorRepository.GetActiveBySpecialisationAsync(specialisation);

        return mapper.Map<List<DoctorDto>>(doctors);
    }

    public async Task<DoctorAvailabilityDto> GetDoctorAvailabilityAsync(
        int doctorId,
        DateTime date)
    {
        ValidateDoctorId(doctorId);

        var availabilityDate = date.Date;

        if (availabilityDate < DateTime.Today)
        {
            throw new BusinessRuleException(
                "Cannot check availability for past dates.");
        }

        if (IsDoctor())
        {
            var loggedInDoctor = await GetLoggedInDoctorAsync();

            if (loggedInDoctor.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(
                    "You are not allowed to access another doctor's availability.");
            }
        }

        var cacheKey = GetDoctorAvailabilityCacheKey(
            doctorId,
            availabilityDate);

        var cachedAvailability = await distributedCache.GetStringAsync(cacheKey);

        if (!string.IsNullOrWhiteSpace(cachedAvailability))
        {
            logger.LogInformation(
                "Doctor availability served from Garnet cache. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                doctorId,
                availabilityDate,
                cacheKey);

            return JsonSerializer.Deserialize<DoctorAvailabilityDto>(cachedAvailability)
                ?? new DoctorAvailabilityDto
                {
                    DoctorId = doctorId,
                    Date = availabilityDate,
                    AvailableSlots = new List<string>()
                };
        }

        logger.LogInformation(
            "Doctor availability cache miss. Reading from SQL Server. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
            doctorId,
            availabilityDate,
            cacheKey);

        var doctor = await doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException(DoctorEntityName, doctorId);

        if (!doctor.IsActive)
        {
            throw new BusinessRuleException("Doctor is inactive.");
        }

        var appointments = await appointmentRepository.GetByDoctorIdAsync(doctorId);

        var bookedSlots = appointments
            .Where(appointment =>
                appointment.ScheduledDate.Date == availabilityDate &&
                appointment.Status != AppointmentStatus.Cancelled.ToString())
            .Select(appointment => appointment.TimeSlots)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var availableSlots = TimeSlots.Slots
            .Where(slot => !bookedSlots.Contains(slot))
            .Where(slot => !IsPastTimeSlot(availabilityDate, slot))
            .ToList();

        var availability = new DoctorAvailabilityDto
        {
            DoctorId = doctorId,
            Date = availabilityDate,
            AvailableSlots = availableSlots
        };

        var serializedAvailability = JsonSerializer.Serialize(availability);

        await distributedCache.SetStringAsync(
            cacheKey,
            serializedAvailability,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        logger.LogInformation(
            "Doctor availability cached for 5 minutes. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
            doctorId,
            availabilityDate,
            cacheKey);

        return availability;
    }

    public async Task InvalidateDoctorAvailabilityCacheAsync(
        int doctorId,
        DateTime date)
    {
        ValidateDoctorId(doctorId);

        var availabilityDate = date.Date;

        var cacheKey = GetDoctorAvailabilityCacheKey(
            doctorId,
            availabilityDate);

        await distributedCache.RemoveAsync(cacheKey);

        logger.LogInformation(
            "Doctor availability cache invalidated. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
            doctorId,
            availabilityDate,
            cacheKey);
    }

    private async Task<Doctor> GetLoggedInDoctorAsync()
    {
        var userId = CurrentUser?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException("Unable to identify logged-in user.");
        }

        var doctor = await doctorRepository.GetByUserIdAsync(userId);

        if (doctor is null)
        {
            throw new EntityNotFoundException(DoctorEntityName, userId);
        }

        return doctor;
    }

    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;

    private bool IsDoctor()
    {
        return CurrentUser?.IsInRole(Roles.Doctor) == true;
    }

    private static void ValidateDoctorId(int id)
    {
        if (id <= 0)
        {
            throw new BusinessRuleException(
                "Please provide a valid doctor reference.");
        }
    }

    private static string GetDoctorAvailabilityCacheKey(
        int doctorId,
        DateTime date)
    {
        return $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
    }

    private static bool IsPastTimeSlot(DateTime scheduledDate, string timeSlot)
    {
        if (scheduledDate.Date != DateTime.Today)
        {
            return false;
        }

        var slotStartTime = GetSlotStartTime(timeSlot);

        var slotStartDateTime = scheduledDate.Date.Add(slotStartTime);

        return slotStartDateTime <= DateTime.Now;
    }

    private static TimeSpan GetSlotStartTime(string timeSlot)
    {
        var startText = timeSlot.Split('-')[0].Trim();

        if (!DateTime.TryParse(
                startText,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var parsedStartTime))
        {
            throw new BusinessRuleException("Invalid time slot format.");
        }

        return parsedStartTime.TimeOfDay;
    }
}