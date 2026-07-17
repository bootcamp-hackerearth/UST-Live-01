using System.Text.Json;
using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace HealthApp.Api.Services.Impl;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IDoctorLeaveRepository _doctorLeaveRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _cache;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(
        AppointmentServiceRepositories repositories,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IDistributedCache cache,
        ILogger<AppointmentService> logger)
    {
        ArgumentNullException.ThrowIfNull(repositories);
        _appointmentRepository = repositories.AppointmentRepository;
        _patientRepository = repositories.PatientRepository;
        _doctorRepository = repositories.DoctorRepository;
        _doctorLeaveRepository = repositories.DoctorLeaveRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _cache = cache;
        _logger = logger;
    }

    public async Task<PagedResultDto<AppointmentDto>> GetAppointmentsAsync(
        AppointmentFilterDto filter)
    {
        filter ??= new AppointmentFilterDto();
        if (filter.Date.HasValue &&
            (filter.FromDate.HasValue || filter.ToDate.HasValue))
        {
            throw new InvalidRequestException(
                "Use either exact date or date range, not both.");
        }

        if (filter.FromDate.HasValue && filter.ToDate.HasValue &&
            filter.FromDate.Value > filter.ToDate.Value)
        {
            throw new InvalidRequestException(
                "From date cannot be greater than to date.");
        }

        var (appointments, totalCount) =
            await _appointmentRepository.GetAppointmentsAsync(filter);
        foreach (var appointment in appointments)
        {
            await LoadAppointmentNavigationDataAsync(appointment);
        }

        return new PagedResultDto<AppointmentDto>
        {
            Items = _mapper.Map<List<AppointmentDto>>(appointments),
            PageNumber = filter.PageNumber,
            PageSize = filter.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new InvalidRequestException(
                "Valid appointment id is required.");
        }

        var appointment = await _appointmentRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Appointment", id);
        await LoadAppointmentNavigationDataAsync(appointment);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> BookAppointmentAsync(
        AppointmentCreateDto dto)
    {
        ValidateBookingRequest(dto);
        var date = DateOnly.FromDateTime(dto.ScheduledDate);
        ValidateScheduledDate(date);
        var patient = await GetPatientForBookingAsync(dto.PatientId);
        var userId = await GetPatientUserIdForBookingAsync(dto.PatientId);
        var doctor = await GetDoctorForBookingAsync(
            dto.DoctorId,
            dto.PatientId);
        await EnsureDoctorIsNotOnLeaveAsync(dto, date);
        var slot = dto.TimeSlot.Trim();
        await EnsureNoBookingConflictsAsync(
            dto.PatientId,
            dto.DoctorId,
            date,
            slot);
        var created = await CreateAppointmentAsync(
            dto,
            date,
            slot,
            patient,
            doctor);
        await PublishAppointmentBookedEventAsync(
            created,
            userId,
            patient,
            doctor);
        LogAppointmentBooked(created);
        return _mapper.Map<AppointmentDto>(created);
    }

    public async Task UpdateAppointmentStatusAsync(
        int id,
        AppointmentStatus status,
        string? cancellationReason = null)
    {
        if (id <= 0)
        {
            throw new InvalidRequestException(
                "Valid appointment id is required.");
        }

        var appointment = await _appointmentRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Appointment", id);
        if (appointment.Status == AppointmentStatus.Completed)
        {
            throw new BusinessRuleViolationException(
                "Completed appointment status cannot be changed.");
        }

        if (status == AppointmentStatus.Cancelled &&
            string.IsNullOrWhiteSpace(cancellationReason))
        {
            throw new InvalidRequestException(
                "Cancellation reason is required.");
        }

        var previousStatus = appointment.Status;
        appointment.Status = status;
        appointment.CancellationReason =
            status == AppointmentStatus.Cancelled
                ? cancellationReason?.Trim()
                : null;
        await _appointmentRepository.Update(id, appointment);
        await RemoveDoctorSlotsCacheAsync(
            appointment.DoctorId,
            appointment.ScheduledDate);

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Appointment {AppointmentId} status changed from {PreviousAppointmentStatus} to {AppointmentStatus} for doctor {DoctorId} and patient {PatientId}. Event type: {EventType}",
                appointment.AppointmentId,
                previousStatus,
                appointment.Status,
                appointment.DoctorId,
                appointment.PatientId,
                "AppointmentStatusChanged");
        }
    }

    public async Task<DoctorAvailabilityDto> GetDoctorAvailabilityAsync(
        int doctorId,
        DateOnly date)
    {
        ValidateAvailabilityRequest(doctorId, date);
        await EnsureDoctorIsAvailableForLookupAsync(doctorId);
        var key = GetDoctorSlotsCacheKey(doctorId, date);
        var cached = await GetCachedAvailabilityAsync(key, doctorId, date);
        if (cached != null)
        {
            return cached;
        }

        var availability = await BuildDoctorAvailabilityAsync(doctorId, date);
        await CacheDoctorAvailabilityAsync(
            key,
            availability,
            doctorId,
            date);
        return availability;
    }

    public async Task DeleteAppointmentAsync(int id)
    {
        if (id <= 0)
        {
            throw new InvalidRequestException(
                "Valid appointment id is required.");
        }

        var appointment = await _appointmentRepository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Appointment", id);
        if (appointment.Status != AppointmentStatus.Cancelled)
        {
            throw new BusinessRuleViolationException(
                "Only cancelled appointments can be deleted.");
        }

        if (!await _appointmentRepository.DeleteAsync(id))
        {
            throw new BusinessRuleViolationException(
                "Unable to delete appointment.");
        }

        await RemoveDoctorSlotsCacheAsync(
            appointment.DoctorId,
            appointment.ScheduledDate);
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Cancelled appointment {AppointmentId} deleted for doctor {DoctorId} and patient {PatientId}. Event type: {EventType}",
                appointment.AppointmentId,
                appointment.DoctorId,
                appointment.PatientId,
                "AppointmentDeleted");
        }
    }

    private static void ValidateAvailabilityRequest(int doctorId, DateOnly date)
    {
        if (doctorId <= 0)
        {
            throw new InvalidRequestException("Valid doctor id is required.");
        }

        if (date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new BusinessRuleViolationException("Past date is not allowed.");
        }
    }

    private async Task EnsureDoctorIsAvailableForLookupAsync(int doctorId)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);
        if (!doctor.IsActive)
        {
            throw new BusinessRuleViolationException("Doctor is inactive.");
        }
    }

    private async Task<DoctorAvailabilityDto?> GetCachedAvailabilityAsync(
        string cacheKey,
        int doctorId,
        DateOnly date)
    {
        var json = await _cache.GetStringAsync(cacheKey);
        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        try
        {
            var cached = JsonSerializer.Deserialize<DoctorAvailabilityDto>(json);
            if (cached != null)
            {
                LogAvailabilityCacheHit(doctorId, date);
            }

            return cached;
        }
        catch (JsonException exception)
        {
            LogInvalidAvailabilityCache(exception, doctorId, date);
            await _cache.RemoveAsync(cacheKey);
            return null;
        }
    }

    private async Task<DoctorAvailabilityDto> BuildDoctorAvailabilityAsync(
        int doctorId,
        DateOnly date)
    {
        var leave = await _doctorLeaveRepository.GetLeaveForDateAsync(
            doctorId,
            date);
        return leave != null
            ? CreateLeaveAvailability(doctorId, date, leave)
            : await CreateRegularAvailabilityAsync(doctorId, date);
    }

    private static DoctorAvailabilityDto CreateLeaveAvailability(
        int doctorId,
        DateOnly date,
        DoctorLeave leave)
    {
        return new DoctorAvailabilityDto
        {
            DoctorId = doctorId,
            Date = date,
            IsDoctorOnLeave = true,
            Message = $"Doctor is on leave from {leave.StartDate:dd MMM yyyy} " +
                      $"to {leave.EndDate:dd MMM yyyy}.",
            Slots = TimeSlots.Slots.Select(slot =>
                new DoctorAvailabilitySlotDto
                {
                    TimeSlot = slot,
                    IsAvailable = false,
                    Status = "DoctorOnLeave"
                }).ToList()
        };
    }

    private async Task<DoctorAvailabilityDto> CreateRegularAvailabilityAsync(
        int doctorId,
        DateOnly date)
    {
        var slots = new List<DoctorAvailabilitySlotDto>();
        foreach (var slot in TimeSlots.Slots)
        {
            var booked = await _appointmentRepository.IsDoctorSlotBookedAsync(
                doctorId,
                date,
                slot);
            slots.Add(new DoctorAvailabilitySlotDto
            {
                TimeSlot = slot,
                IsAvailable = !booked,
                Status = booked ? "Booked" : "Available"
            });
        }

        return new DoctorAvailabilityDto
        {
            DoctorId = doctorId,
            Date = date,
            IsDoctorOnLeave = false,
            Message = "Doctor is available on the selected date.",
            Slots = slots
        };
    }

    private async Task CacheDoctorAvailabilityAsync(
        string cacheKey,
        DoctorAvailabilityDto availability,
        int doctorId,
        DateOnly date)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(availability),
            options);
        LogAvailabilityCached(doctorId, date, availability.IsDoctorOnLeave);
    }

    private void LogAvailabilityCacheHit(int doctorId, DateOnly date)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Doctor availability cache hit for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
            doctorId,
            date,
            "DoctorAvailabilityCacheHit");
    }

    private void LogInvalidAvailabilityCache(
        JsonException exception,
        int doctorId,
        DateOnly date)
    {
        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(
                exception,
                "Invalid cached doctor availability was removed for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                doctorId,
                date,
                "DoctorAvailabilityCacheInvalid");
        }
    }

    private void LogAvailabilityCached(
        int doctorId,
        DateOnly date,
        bool isDoctorOnLeave)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Doctor availability cached for doctor {DoctorId} on {AvailabilityDate}. Doctor on leave: {IsDoctorOnLeave}. Event type: {EventType}",
            doctorId,
            date,
            isDoctorOnLeave,
            "DoctorAvailabilityCached");
    }

    private static void ValidateBookingRequest(AppointmentCreateDto dto)
    {
        if (dto == null)
        {
            throw new InvalidRequestException("Appointment data is required.");
        }

        if (dto.PatientId <= 0)
        {
            throw new InvalidRequestException("Valid patient is required.");
        }

        if (dto.DoctorId <= 0)
        {
            throw new InvalidRequestException("Valid doctor is required.");
        }

        if (dto.ScheduledDate == default)
        {
            throw new InvalidRequestException("Scheduled date is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.TimeSlot))
        {
            throw new InvalidRequestException("Time slot is required.");
        }
    }

    private static void ValidateScheduledDate(DateOnly date)
    {
        if (date < DateOnly.FromDateTime(DateTime.Today))
        {
            throw new BusinessRuleViolationException("Past date is not allowed.");
        }
    }

    private async Task<Patient> GetPatientForBookingAsync(int patientId)
    {
        return await _patientRepository.GetByIdAsync(patientId)
            ?? throw new EntityNotFoundException("Patient", patientId);
    }

    private async Task<string> GetPatientUserIdForBookingAsync(int patientId)
    {
        var userId = await _patientRepository.GetPatientUserIdAsync(
            patientId,
            CancellationToken.None);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return userId;
        }

        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(
                "Appointment booking was rejected because patient {PatientId} has no linked user account. Event type: {EventType}",
                patientId,
                "AppointmentBookingRejected");
        }

        throw new BusinessRuleViolationException(
            "Patient user account is not linked.");
    }

    private async Task<Doctor> GetDoctorForBookingAsync(
        int doctorId,
        int patientId)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId)
            ?? throw new EntityNotFoundException("Doctor", doctorId);
        if (doctor.IsActive)
        {
            return doctor;
        }

        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(
                "Appointment booking was rejected because doctor {DoctorId} is inactive. Patient {PatientId}. Event type: {EventType}",
                doctorId,
                patientId,
                "AppointmentBookingRejected");
        }

        throw new BusinessRuleViolationException("Selected doctor is inactive.");
    }

    private async Task EnsureDoctorIsNotOnLeaveAsync(
        AppointmentCreateDto dto,
        DateOnly date)
    {
        var leave = await _doctorLeaveRepository.GetLeaveForDateAsync(
            dto.DoctorId,
            date);
        if (leave == null)
        {
            return;
        }

        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(
                "Appointment booking was rejected because doctor {DoctorId} is on leave on {ScheduledDate}. Patient {PatientId}, leave {DoctorLeaveId}. Event type: {EventType}",
                dto.DoctorId,
                date,
                dto.PatientId,
                leave.DoctorLeaveId,
                "AppointmentBookingRejectedDoctorOnLeave");
        }

        throw new BusinessRuleViolationException(
            $"The selected doctor is on leave from {leave.StartDate:dd MMM yyyy} " +
            $"to {leave.EndDate:dd MMM yyyy}. Please select another appointment date.");
    }

    private async Task EnsureNoBookingConflictsAsync(
        int patientId,
        int doctorId,
        DateOnly date,
        string slot)
    {
        if (await _appointmentRepository.HasAppointmentWithDoctorOnSameDayAsync(
                patientId,
                doctorId,
                date))
        {
            throw new BusinessRuleViolationException(
                "Patient already has an appointment with this doctor on the same day.");
        }

        if (await _appointmentRepository.HasPatientSlotConflictAsync(
                patientId,
                date,
                slot))
        {
            throw new BusinessRuleViolationException(
                "Patient already has an appointment in this time slot.");
        }

        if (await _appointmentRepository.IsDoctorSlotBookedAsync(
                doctorId,
                date,
                slot))
        {
            throw new BusinessRuleViolationException(
                "Doctor is already booked for this time slot.");
        }
    }

    private async Task<Appointment> CreateAppointmentAsync(
        AppointmentCreateDto dto,
        DateOnly date,
        string slot,
        Patient patient,
        Doctor doctor)
    {
        var appointment = _mapper.Map<Appointment>(dto);
        appointment.PatientId = dto.PatientId;
        appointment.DoctorId = dto.DoctorId;
        appointment.ScheduledDate = date;
        appointment.TimeSlot = slot;
        appointment.Status = AppointmentStatus.Pending;
        appointment.CancellationReason = null;
        var created = await _appointmentRepository.Add(appointment);
        created.Patient = patient;
        created.Doctor = doctor;
        await RemoveDoctorSlotsCacheAsync(
            created.DoctorId,
            created.ScheduledDate);
        return created;
    }

    private async Task PublishAppointmentBookedEventAsync(
        Appointment appointment,
        string patientUserId,
        Patient patient,
        Doctor doctor)
    {
        var message = new AppointmentBookedEvent(
            appointment.AppointmentId,
            appointment.PatientId,
            patientUserId,
            patient.FullName ?? string.Empty,
            appointment.DoctorId,
            doctor.FullName ?? string.Empty,
            appointment.ScheduledDate.ToDateTime(TimeOnly.MinValue),
            appointment.TimeSlot ?? string.Empty);
        await _publishEndpoint.Publish(message);
    }

    private void LogAppointmentBooked(Appointment appointment)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Appointment {AppointmentId} booked for patient {PatientId} with doctor {DoctorId} on {ScheduledDate}. Event type: {EventType}",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ScheduledDate,
                "AppointmentBooked");
        }
    }

    private async Task LoadAppointmentNavigationDataAsync(
        Appointment appointment)
    {
        appointment.Patient ??=
            await _patientRepository.GetByIdAsync(appointment.PatientId);
        appointment.Doctor ??=
            await _doctorRepository.GetByIdAsync(appointment.DoctorId);
    }

    private static string GetDoctorSlotsCacheKey(int doctorId, DateOnly date)
    {
        return $"doctor:{doctorId}:slots:{date:yyyy-MM-dd}";
    }

    private async Task RemoveDoctorSlotsCacheAsync(
        int doctorId,
        DateOnly date)
    {
        await _cache.RemoveAsync(GetDoctorSlotsCacheKey(doctorId, date));
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Doctor availability cache invalidated for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                doctorId,
                date,
                "DoctorAvailabilityCacheInvalidated");
        }
    }
}
