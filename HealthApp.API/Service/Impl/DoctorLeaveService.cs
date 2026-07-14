using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;

namespace HealthApp.API.Service.Impl;

public class DoctorLeaveService(
    IDoctorLeaveRepository doctorLeaveRepository,
    IDoctorRepository doctorRepository,
    IAppointmentRepository appointmentRepository,
    HealthAppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    IDistributedCache distributedCache,
    IMapper mapper,
    ILogger<DoctorLeaveService> logger) : IDoctorLeaveService
{
    private const string DoctorEntityName = "Doctor";

    private const int MaximumCancellationReasonLength = 200;

    private const string CancellationReasonPrefix =
        "Cancelled because the doctor is unavailable due to scheduled leave: ";

    public async Task<DoctorLeaveImpactDto> PreviewMyLeaveImpactAsync(
        CreateDoctorLeaveDto dto,
        CancellationToken ct = default)
    {
        if (dto is null)
        {
            throw new BusinessRuleException(
                "Leave details are required.");
        }

        var doctor = await GetLoggedInDoctorAsync(ct);

        if (!doctor.IsActive)
        {
            throw new ForbiddenAccessException(
                "Inactive doctors are not allowed to create leave.");
        }

        ValidateLeaveRequest(dto);

        var startDate = dto.StartDate.Date;
        var endDate = dto.EndDate.Date;

        await EnsureLeaveDoesNotOverlapAsync(
            doctor.DoctorId,
            startDate,
            endDate,
            ct);

        var affectedAppointmentCount =
            await appointmentRepository
                .CountActiveAppointmentsInDateRangeAsync(
                    doctor.DoctorId,
                    startDate,
                    endDate,
                    ct);

        var requiresConfirmation =
            affectedAppointmentCount > 0;

        logger.LogInformation(
            "Doctor leave impact preview completed. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}, AffectedAppointmentCount: {AffectedAppointmentCount}, RequiresConfirmation: {RequiresConfirmation}",
            doctor.DoctorId,
            startDate,
            endDate,
            affectedAppointmentCount,
            requiresConfirmation);

        return new DoctorLeaveImpactDto
        {
            AffectedAppointmentCount =
                affectedAppointmentCount,

            RequiresConfirmation =
                requiresConfirmation,

            Message = requiresConfirmation
                ? $"This leave affects {affectedAppointmentCount} active appointment{GetPluralSuffix(affectedAppointmentCount)}."
                : "No active appointments are affected."
        };
    }

    public async Task<DoctorLeaveCreationResultDto> CreateMyLeaveAsync(
        CreateDoctorLeaveDto dto,
        CancellationToken ct = default)
    {
        if (dto is null)
        {
            throw new BusinessRuleException(
                "Leave details are required.");
        }

        var doctor = await GetLoggedInDoctorAsync(ct);

        if (!doctor.IsActive)
        {
            throw new ForbiddenAccessException(
                "Inactive doctors are not allowed to create leave.");
        }

        ValidateLeaveRequest(dto);

        var startDate = dto.StartDate.Date;
        var endDate = dto.EndDate.Date;
        var leaveReason = dto.Reason.Trim();

        await using var transaction =
            await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            /*
             * Repeat the overlap check inside the final transaction.
             * The preview may have happened several seconds earlier.
             */
            await EnsureLeaveDoesNotOverlapAsync(
                doctor.DoctorId,
                startDate,
                endDate,
                ct);

            /*
             * Re-query appointments during final creation.
             * A patient may have booked after the preview response.
             */
            var affectedAppointments =
                await appointmentRepository
                    .GetActiveAppointmentsInDateRangeAsync(
                        doctor.DoctorId,
                        startDate,
                        endDate,
                        ct);

            var affectedAppointmentCount =
                affectedAppointments.Count;

            if (affectedAppointmentCount > 0 &&
                !dto.ConfirmAppointmentCancellation)
            {
                logger.LogWarning(
                    "Doctor leave creation requires appointment cancellation confirmation. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}, AffectedAppointmentCount: {AffectedAppointmentCount}",
                    doctor.DoctorId,
                    startDate,
                    endDate,
                    affectedAppointmentCount);

                throw new ConflictException(
                    $"This leave affects {affectedAppointmentCount} active appointment{GetPluralSuffix(affectedAppointmentCount)}. " +
                    "Confirmation is required before creating the leave.");
            }

            var doctorLeave = new DoctorLeave
            {
                DoctorId = doctor.DoctorId,
                Doctor = doctor,
                StartDate = startDate,
                EndDate = endDate,
                Reason = leaveReason,
                CreatedDate = DateTime.Now
            };

            await dbContext.DoctorLeaves.AddAsync(
                doctorLeave,
                ct);

            var cancellationReason =
                BuildAppointmentCancellationReason(
                    leaveReason);

            foreach (var appointment in affectedAppointments)
            {
                appointment.Status =
                    AppointmentStatus.Cancelled.ToString();

                appointment.CancellationReason =
                    cancellationReason;
            }

            /*
             * DoctorLeave and all affected Appointment changes
             * are saved together in one database operation.
             */
            await dbContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            logger.LogInformation(
                "Doctor leave created successfully. DoctorLeaveId: {DoctorLeaveId}, DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}, CancelledAppointmentCount: {CancelledAppointmentCount}",
                doctorLeave.DoctorLeaveId,
                doctorLeave.DoctorId,
                doctorLeave.StartDate,
                doctorLeave.EndDate,
                affectedAppointmentCount);

            if (affectedAppointmentCount > 0)
            {
                logger.LogInformation(
                    "Affected appointments cancelled because of doctor leave. DoctorLeaveId: {DoctorLeaveId}, DoctorId: {DoctorId}, CancelledAppointmentCount: {CancelledAppointmentCount}, CancellationReason: {CancellationReason}",
                    doctorLeave.DoctorLeaveId,
                    doctor.DoctorId,
                    affectedAppointmentCount,
                    cancellationReason);
            }

            /*
             * The database transaction is already committed.
             * Garnet failure must not undo leave creation or cancellations.
             */
            await InvalidateAvailabilityCacheAsync(
                doctorLeave.DoctorId,
                doctorLeave.StartDate,
                doctorLeave.EndDate,
                ct);

            return new DoctorLeaveCreationResultDto
            {
                Leave = mapper.Map<DoctorLeaveDto>(
                    doctorLeave),

                CancelledAppointmentCount =
                    affectedAppointmentCount,

                Message = affectedAppointmentCount > 0
                    ? $"Leave created and {affectedAppointmentCount} affected appointment{GetPluralSuffix(affectedAppointmentCount)} cancelled."
                    : "Leave created successfully."
            };
        }
        catch
        {
            await transaction.RollbackAsync(
                CancellationToken.None);

            throw;
        }
    }

    public async Task<List<DoctorLeaveDto>> GetMyLeaveHistoryAsync(
        CancellationToken ct = default)
    {
        var doctor = await GetLoggedInDoctorAsync(ct);

        var leaves =
            await doctorLeaveRepository.GetByDoctorIdAsync(
                doctor.DoctorId,
                ct);

        logger.LogInformation(
            "Doctor leave history retrieved. DoctorId: {DoctorId}, LeaveCount: {LeaveCount}",
            doctor.DoctorId,
            leaves.Count);

        return mapper.Map<List<DoctorLeaveDto>>(leaves);
    }

    public async Task<List<DoctorLeaveDto>>
        GetDoctorLeaveHistoryAsync(
            int doctorId,
            CancellationToken ct = default)
    {
        var doctor = await ValidateDoctorExistsAsync(
            doctorId,
            ct);

        var leaves =
            await doctorLeaveRepository.GetByDoctorIdAsync(
                doctor.DoctorId,
                ct);

        logger.LogInformation(
            "Doctor leave history retrieved for admin. DoctorId: {DoctorId}, LeaveCount: {LeaveCount}",
            doctor.DoctorId,
            leaves.Count);

        return mapper.Map<List<DoctorLeaveDto>>(leaves);
    }

    public async Task<DoctorLeaveStatusDto>
        GetDoctorLeaveStatusAsync(
            int doctorId,
            DateTime date,
            CancellationToken ct = default)
    {
        var doctor = await ValidateDoctorExistsAsync(
            doctorId,
            ct);

        var selectedDate = date.Date;

        var doctorLeave =
            await doctorLeaveRepository.GetLeaveForDateAsync(
                doctor.DoctorId,
                selectedDate,
                ct);

        if (doctorLeave is null)
        {
            return new DoctorLeaveStatusDto
            {
                DoctorId = doctor.DoctorId,
                Date = selectedDate,
                IsOnLeave = false,
                Message =
                    "Doctor is available on the selected date.",
                Leave = null
            };
        }

        return new DoctorLeaveStatusDto
        {
            DoctorId = doctor.DoctorId,
            Date = selectedDate,
            IsOnLeave = true,
            Message =
                "Doctor is on leave on the selected date.",
            Leave = mapper.Map<DoctorLeaveDto>(
                doctorLeave)
        };
    }

    private async Task EnsureLeaveDoesNotOverlapAsync(
        int doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct)
    {
        var hasOverlap =
            await doctorLeaveRepository
                .HasOverlappingLeaveAsync(
                    doctorId,
                    startDate,
                    endDate,
                    ct);

        if (!hasOverlap)
        {
            return;
        }

        logger.LogWarning(
            "Doctor leave request rejected because the selected range overlaps an existing leave. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
            doctorId,
            startDate,
            endDate);

        throw new ConflictException(
            "The selected leave period overlaps with an existing leave.");
    }

    private async Task<Doctor> GetLoggedInDoctorAsync(
        CancellationToken ct)
    {
        var userId = CurrentUser?
            .FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ForbiddenAccessException(
                "Unable to identify the logged-in doctor.");
        }

        var doctor =
            await doctorRepository.GetByUserIdAsync(
                userId,
                ct);

        if (doctor is null)
        {
            throw new EntityNotFoundException(
                DoctorEntityName,
                userId);
        }

        return doctor;
    }

    private async Task<Doctor> ValidateDoctorExistsAsync(
        int doctorId,
        CancellationToken ct)
    {
        if (doctorId <= 0)
        {
            throw new BusinessRuleException(
                "Please provide a valid doctor reference.");
        }

        return await doctorRepository.GetByIdAsync(
                   doctorId,
                   ct)
               ?? throw new EntityNotFoundException(
                   DoctorEntityName,
                   doctorId);
    }

    private static void ValidateLeaveRequest(
        CreateDoctorLeaveDto dto)
    {
        if (dto.StartDate == default)
        {
            throw new BusinessRuleException(
                "Leave start date is required.");
        }

        if (dto.EndDate == default)
        {
            throw new BusinessRuleException(
                "Leave end date is required.");
        }

        if (dto.StartDate.Date < DateTime.Today)
        {
            throw new BusinessRuleException(
                "Leave start date cannot be in the past.");
        }

        if (dto.EndDate.Date <
            dto.StartDate.Date)
        {
            throw new BusinessRuleException(
                "Leave end date cannot be before the start date.");
        }

        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            throw new BusinessRuleException(
                "Leave reason is required.");
        }

        var trimmedReason = dto.Reason.Trim();

        if (trimmedReason.Length < 3)
        {
            throw new BusinessRuleException(
                "Leave reason must contain at least 3 characters.");
        }

        if (trimmedReason.Length > 500)
        {
            throw new BusinessRuleException(
                "Leave reason must not exceed 500 characters.");
        }
    }

    private static string BuildAppointmentCancellationReason(
        string leaveReason)
    {
        var normalizedReason = leaveReason.Trim();

        var maximumLeaveReasonLength =
            MaximumCancellationReasonLength -
            CancellationReasonPrefix.Length;

        if (maximumLeaveReasonLength < 0)
        {
            maximumLeaveReasonLength = 0;
        }

        if (normalizedReason.Length >
            maximumLeaveReasonLength)
        {
            normalizedReason =
                normalizedReason[
                    ..maximumLeaveReasonLength];
        }

        return CancellationReasonPrefix +
               normalizedReason;
    }

    private async Task InvalidateAvailabilityCacheAsync(
        int doctorId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken ct)
    {
        var currentDate = startDate.Date;
        var finalDate = endDate.Date;

        while (currentDate <= finalDate)
        {
            var cacheKey =
                GetDoctorAvailabilityCacheKey(
                    doctorId,
                    currentDate);

            try
            {
                await distributedCache.RemoveAsync(
                    cacheKey,
                    ct);

                logger.LogInformation(
                    "Doctor availability cache invalidated after leave creation. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                    doctorId,
                    currentDate,
                    cacheKey);
            }
            catch (OperationCanceledException)
                when (ct.IsCancellationRequested)
            {
                logger.LogWarning(
                    "Doctor availability cache invalidation was cancelled after leave creation. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                    doctorId,
                    currentDate,
                    cacheKey);

                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Doctor leave and appointment cancellations succeeded, but availability cache invalidation failed. DoctorId: {DoctorId}, Date: {Date}, CacheKey: {CacheKey}",
                    doctorId,
                    currentDate,
                    cacheKey);
            }

            currentDate =
                currentDate.AddDays(1);
        }
    }

    private static string GetDoctorAvailabilityCacheKey(
        int doctorId,
        DateTime date)
    {
        return
            $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
    }

    private static string GetPluralSuffix(int count)
    {
        return count == 1
            ? string.Empty
            : "s";
    }

    private ClaimsPrincipal? CurrentUser =>
        httpContextAccessor.HttpContext?.User;
}