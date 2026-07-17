using AutoMapper;
using HealthApp.Api.Data;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Dependencies;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace HealthApp.Api.Services.Impl;

public class DoctorLeaveService : IDoctorLeaveService
{
    private readonly HealthAppDbContext _context;
    private readonly IDoctorLeaveRepository _doctorLeaveRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DoctorLeaveService> _logger;

    public DoctorLeaveService(
        DoctorLeaveServiceDependencies dependencies,
        IMapper mapper,
        IPublishEndpoint publishEndpoint,
        IDistributedCache cache,
        ILogger<DoctorLeaveService> logger)
    {
        ArgumentNullException.ThrowIfNull(dependencies);
        _context = dependencies.Context;
        _doctorLeaveRepository = dependencies.DoctorLeaveRepository;
        _doctorRepository = dependencies.DoctorRepository;
        _appointmentRepository = dependencies.AppointmentRepository;
        _patientRepository = dependencies.PatientRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _cache = cache;
        _logger = logger;
    }

    public async Task<DoctorLeavePreviewDto> PreviewLeaveAsync(
        int doctorId,
        DoctorLeaveCreateDto dto,
        CancellationToken ct = default)
    {
        ValidateDoctorId(doctorId);
        if (dto == null)
        {
            throw new InvalidRequestException("Doctor leave data is required.");
        }

        ValidateLeave(dto);
        await GetDoctorAsync(doctorId, ct);
        await EnsureLeaveDoesNotOverlapAsync(doctorId, dto, ct);
        var appointments = await _appointmentRepository
            .GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId, dto.StartDate, dto.EndDate, ct);
        var pending = appointments.Count(x =>
            x.Status == AppointmentStatus.Pending);
        var confirmed = appointments.Count(x =>
            x.Status == AppointmentStatus.Confirmed);
        var total = pending + confirmed;

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Doctor leave preview completed for doctor {DoctorId} from {StartDate} to {EndDate}. Pending {PendingAppointmentCount}, confirmed {ConfirmedAppointmentCount}, total affected {AffectedAppointmentCount}. Event type: {EventType}",
                doctorId, dto.StartDate, dto.EndDate, pending, confirmed,
                total, "DoctorLeavePreviewed");
        }

        return new DoctorLeavePreviewDto
        {
            DoctorId = doctorId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            PendingAppointmentCount = pending,
            ConfirmedAppointmentCount = confirmed,
            TotalAffectedAppointmentCount = total,
            Message = total > 0
                ? $"{total} active appointment(s) will be cancelled if this leave is confirmed."
                : "No active appointments will be affected by this leave."
        };
    }

    public async Task<DoctorLeaveCreationResultDto> CreateLeaveAsync(
        int doctorId,
        DoctorLeaveCreateDto dto,
        CancellationToken ct = default)
    {
        ValidateDoctorId(doctorId);
        if (dto == null)
        {
            throw new InvalidRequestException("Doctor leave data is required.");
        }

        ValidateLeave(dto);
        var doctor = await GetDoctorAsync(doctorId, ct);
        await EnsureLeaveDoesNotOverlapAsync(doctorId, dto, ct);
        var appointments = await _appointmentRepository
            .GetActiveAppointmentsForDoctorDateRangeAsync(
                doctorId, dto.StartDate, dto.EndDate, ct);
        var reason = $"Appointment cancelled because " +
            $"{doctor.FullName ?? "the doctor"} is unavailable from " +
            $"{dto.StartDate:dd MMM yyyy} to {dto.EndDate:dd MMM yyyy}.";
        var leave = _mapper.Map<DoctorLeave>(dto);
        leave.DoctorId = doctorId;
        leave.Doctor = doctor;
        leave.Reason = dto.Reason.Trim();
        leave.CreatedAtUtc = DateTime.UtcNow;

        await using var transaction =
            await _context.Database.BeginTransactionAsync(ct);
        try
        {
            var created = await _doctorLeaveRepository.Add(leave, ct);
            await _appointmentRepository.CancelAppointmentsAsync(
                appointments, reason, ct);
            await transaction.CommitAsync(ct);
            var cancelledAppointmentIds = appointments
                .Select(x => x.AppointmentId)
                .ToList();

            LogLeaveCreated(
                created, doctorId, cancelledAppointmentIds.Count);
            LogCancelledAppointmentIds(
                created.DoctorLeaveId, cancelledAppointmentIds);
            await InvalidateLeaveDateCachesAsync(
                doctorId, dto.StartDate, dto.EndDate, ct);
            await PublishCancellationEventsAsync(
                appointments, doctor, created, ct);

            return new DoctorLeaveCreationResultDto
            {
                Leave = _mapper.Map<DoctorLeaveDto>(created),
                CancelledAppointmentCount = cancelledAppointmentIds.Count,
                CancelledAppointmentIds = cancelledAppointmentIds
            };
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw new InvalidOperationException(
                $"Failed to create doctor leave for doctor {doctorId} " +
                $"from {dto.StartDate:yyyy-MM-dd} " +
                $"to {dto.EndDate:yyyy-MM-dd}.",
                exception);
        }
    }

    public async Task<IEnumerable<DoctorLeaveDto>> GetDoctorLeavesAsync(
        int doctorId,
        CancellationToken ct = default)
    {
        ValidateDoctorId(doctorId);
        await GetDoctorAsync(doctorId, ct);
        var leaves = await _doctorLeaveRepository
            .GetByDoctorIdAsync(doctorId, ct);
        return _mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves);
    }

    public async Task<DoctorLeaveDto?> GetLeaveForDateAsync(
        int doctorId,
        DateOnly date,
        CancellationToken ct = default)
    {
        ValidateDoctorId(doctorId);
        await GetDoctorAsync(doctorId, ct);
        var leave = await _doctorLeaveRepository
            .GetLeaveForDateAsync(doctorId, date, ct);
        return leave == null ? null : _mapper.Map<DoctorLeaveDto>(leave);
    }

    public async Task<bool> IsDoctorOnLeaveAsync(
        int doctorId,
        DateOnly date,
        CancellationToken ct = default)
    {
        ValidateDoctorId(doctorId);
        await GetDoctorAsync(doctorId, ct);
        return await _doctorLeaveRepository
            .IsDoctorOnLeaveAsync(doctorId, date, ct);
    }

    private async Task EnsureLeaveDoesNotOverlapAsync(
        int doctorId,
        DoctorLeaveCreateDto dto,
        CancellationToken ct)
    {
        var overlaps = await _doctorLeaveRepository.HasOverlappingLeaveAsync(
            doctorId, dto.StartDate, dto.EndDate, ct);
        if (!overlaps)
        {
            return;
        }

        if (_logger.IsEnabled(LogLevel.Warning))
        {
            _logger.LogWarning(
                "Doctor leave request rejected because doctor {DoctorId} has an overlapping leave from {StartDate} to {EndDate}. Event type: {EventType}",
                doctorId, dto.StartDate, dto.EndDate,
                "DoctorLeaveOverlapRejected");
        }

        throw new BusinessRuleViolationException(
            "The selected leave dates overlap with an existing leave record.");
    }

    private async Task PublishCancellationEventsAsync(
        IEnumerable<Appointment> appointments,
        Doctor doctor,
        DoctorLeave leave,
        CancellationToken ct)
    {
        foreach (var appointment in appointments)
        {
            try
            {
                var userId = await _patientRepository.GetPatientUserIdAsync(
                    appointment.PatientId, ct);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    LogNotificationSkipped(appointment, leave);
                    continue;
                }

                var cancellationEvent =
                    new AppointmentCancelledByDoctorLeaveEvent(
                        appointment.AppointmentId,
                        appointment.PatientId,
                        userId,
                        appointment.Patient?.FullName ?? string.Empty,
                        appointment.DoctorId,
                        doctor.FullName ?? string.Empty,
                        appointment.ScheduledDate.ToDateTime(
                            TimeOnly.MinValue),
                        appointment.TimeSlot ?? string.Empty,
                        leave.StartDate.ToDateTime(TimeOnly.MinValue),
                        leave.EndDate.ToDateTime(TimeOnly.MinValue),
                        leave.Reason);
                await _publishEndpoint.Publish(cancellationEvent, ct);

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Doctor-leave cancellation event published for appointment {AppointmentId}, patient {PatientId}, doctor {DoctorId}, and leave {DoctorLeaveId}. Event type: {EventType}",
                        appointment.AppointmentId, appointment.PatientId,
                        appointment.DoctorId, leave.DoctorLeaveId,
                        "DoctorLeaveCancellationEventPublished");
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(
                        exception,
                        "Doctor leave {DoctorLeaveId} was created, but the cancellation event could not be published for appointment {AppointmentId} and patient {PatientId}. Event type: {EventType}",
                        leave.DoctorLeaveId, appointment.AppointmentId,
                        appointment.PatientId,
                        "DoctorLeaveCancellationEventFailed");
                }
            }
        }
    }

    private async Task InvalidateLeaveDateCachesAsync(
        int doctorId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct)
    {
        var invalidatedDateCount = 0;
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            try
            {
                await _cache.RemoveAsync(
                    GetDoctorSlotsCacheKey(doctorId, date), ct);
                invalidatedDateCount++;
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Doctor availability cache invalidated for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                        doctorId, date,
                        "DoctorAvailabilityCacheInvalidated");
                }
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning(
                        exception,
                        "Doctor leave was created, but availability cache invalidation failed for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                        doctorId, date,
                        "DoctorAvailabilityCacheInvalidationFailed");
                }
            }
        }

        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(
                "Doctor availability cache invalidated for {InvalidatedDateCount} date(s) for doctor {DoctorId}, from {StartDate} to {EndDate}. Event type: {EventType}",
                invalidatedDateCount, doctorId, startDate, endDate,
                "DoctorLeaveCacheInvalidationCompleted");
        }
    }

    private void LogLeaveCreated(
        DoctorLeave created,
        int doctorId,
        int cancelledAppointmentCount)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Doctor leave {DoctorLeaveId} created for doctor {DoctorId} from {StartDate} to {EndDate}. Cancelled {CancelledAppointmentCount} appointment(s). Event type: {EventType}",
            created.DoctorLeaveId, doctorId, created.StartDate,
            created.EndDate, cancelledAppointmentCount,
            "DoctorLeaveCreated");
    }

    private void LogCancelledAppointmentIds(
        int doctorLeaveId,
        List<int> cancelledAppointmentIds)
    {
        if (cancelledAppointmentIds.Count == 0 ||
            !_logger.IsEnabled(LogLevel.Information))
        {
            return;
        }

        _logger.LogInformation(
            "Appointments cancelled for doctor leave {DoctorLeaveId}: {@CancelledAppointmentIds}. Event type: {EventType}",
            doctorLeaveId, cancelledAppointmentIds,
            "AppointmentsCancelledByDoctorLeave");
    }

    private void LogNotificationSkipped(
        Appointment appointment,
        DoctorLeave leave)
    {
        if (!_logger.IsEnabled(LogLevel.Warning))
        {
            return;
        }

        _logger.LogWarning(
            "Doctor-leave notification was not published because patient {PatientId} has no linked user account. Appointment {AppointmentId}, leave {DoctorLeaveId}. Event type: {EventType}",
            appointment.PatientId, appointment.AppointmentId,
            leave.DoctorLeaveId, "DoctorLeaveNotificationSkipped");
    }

    private async Task<Doctor> GetDoctorAsync(
        int doctorId,
        CancellationToken ct)
    {
        return await _doctorRepository.GetByIdAsync(doctorId, ct)
            ?? throw new EntityNotFoundException("Doctor", doctorId);
    }

    private static void ValidateDoctorId(int doctorId)
    {
        if (doctorId <= 0)
        {
            throw new InvalidRequestException("Valid doctor id is required.");
        }
    }

    private static void ValidateLeave(DoctorLeaveCreateDto dto)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (dto.StartDate < today)
        {
            throw new BusinessRuleViolationException(
                "Leave start date cannot be in the past.");
        }

        if (dto.EndDate < dto.StartDate)
        {
            throw new BusinessRuleViolationException(
                "Leave end date cannot be before the start date.");
        }

        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            throw new InvalidRequestException("Leave reason is required.");
        }

        var reason = dto.Reason.Trim();
        if (reason.Length < 3)
        {
            throw new InvalidRequestException(
                "Leave reason must be at least 3 characters long.");
        }

        if (reason.Length > 500)
        {
            throw new InvalidRequestException(
                "Leave reason cannot exceed 500 characters.");
        }
    }

    private static string GetDoctorSlotsCacheKey(
        int doctorId,
        DateOnly date)
    {
        return $"doctor:{doctorId}:slots:{date:yyyy-MM-dd}";
    }
}
