using AutoMapper;
using HealthApp.Api.Data;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;

namespace HealthApp.Api.Services.Impl
{
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
            HealthAppDbContext context,
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            IDistributedCache cache,
            ILogger<DoctorLeaveService> logger)
        {
            _context = context;
            _doctorLeaveRepository = doctorLeaveRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
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
                throw new InvalidRequestException(
                    "Doctor leave data is required.");
            }

            ValidateLeave(dto);
            await GetDoctorAsync(doctorId, ct);
            await EnsureLeaveDoesNotOverlapAsync(doctorId, dto, ct);

            var affectedAppointments = await _appointmentRepository
                .GetActiveAppointmentsForDoctorDateRangeAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    ct);

            var pendingCount = affectedAppointments.Count(appointment =>
                appointment.Status == AppointmentStatus.Pending);

            var confirmedCount = affectedAppointments.Count(appointment =>
                appointment.Status == AppointmentStatus.Confirmed);

            var totalCount = pendingCount + confirmedCount;

            var message = totalCount > 0
                ? $"{totalCount} active appointment(s) will be cancelled if this leave is confirmed."
                : "No active appointments will be affected by this leave.";

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "Doctor leave preview completed for doctor {DoctorId} from {StartDate} to {EndDate}. Pending {PendingAppointmentCount}, confirmed {ConfirmedAppointmentCount}, total affected {AffectedAppointmentCount}. Event type: {EventType}",
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    pendingCount,
                    confirmedCount,
                    totalCount,
                    "DoctorLeavePreviewed");
            }

            return new DoctorLeavePreviewDto
            {
                DoctorId = doctorId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PendingAppointmentCount = pendingCount,
                ConfirmedAppointmentCount = confirmedCount,
                TotalAffectedAppointmentCount = totalCount,
                Message = message
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
                throw new InvalidRequestException(
                    "Doctor leave data is required.");
            }

            ValidateLeave(dto);

            var doctor = await GetDoctorAsync(doctorId, ct);
            await EnsureLeaveDoesNotOverlapAsync(doctorId, dto, ct);

            var affectedAppointments = await _appointmentRepository
                .GetActiveAppointmentsForDoctorDateRangeAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    ct);

            var cancellationReason =
                $"Appointment cancelled because {doctor.FullName ?? "the doctor"} " +
                $"is unavailable from {dto.StartDate:dd MMM yyyy} " +
                $"to {dto.EndDate:dd MMM yyyy}.";

            var doctorLeave = _mapper.Map<DoctorLeave>(dto);
            doctorLeave.DoctorId = doctorId;
            doctorLeave.Doctor = doctor;
            doctorLeave.Reason = dto.Reason.Trim();
            doctorLeave.CreatedAtUtc = DateTime.UtcNow;

            await using var transaction = await _context.Database
                .BeginTransactionAsync(ct);

            try
            {
                var createdLeave = await _doctorLeaveRepository.Add(
                    doctorLeave,
                    ct);

                await _appointmentRepository.CancelAppointmentsAsync(
                    affectedAppointments,
                    cancellationReason,
                    ct);

                await transaction.CommitAsync(ct);

                var cancelledAppointmentIds = affectedAppointments
                    .Select(appointment => appointment.AppointmentId)
                    .ToList();

                var result = new DoctorLeaveCreationResultDto
                {
                    Leave = _mapper.Map<DoctorLeaveDto>(createdLeave),
                    CancelledAppointmentCount = cancelledAppointmentIds.Count,
                    CancelledAppointmentIds = cancelledAppointmentIds
                };

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                        "Doctor leave {DoctorLeaveId} created for doctor {DoctorId} from {StartDate} to {EndDate}. Cancelled {CancelledAppointmentCount} appointment(s). Event type: {EventType}",
                        createdLeave.DoctorLeaveId,
                        doctorId,
                        createdLeave.StartDate,
                        createdLeave.EndDate,
                        cancelledAppointmentIds.Count,
                        "DoctorLeaveCreated");
                }

                if (_logger.IsEnabled(LogLevel.Debug) &&
                    cancelledAppointmentIds.Count > 0)
                {
                    _logger.LogDebug(
                        "Appointments cancelled for doctor leave {DoctorLeaveId}: {@CancelledAppointmentIds}. Event type: {EventType}",
                        createdLeave.DoctorLeaveId,
                        cancelledAppointmentIds,
                        "AppointmentsCancelledByDoctorLeave");
                }

                await InvalidateLeaveDateCachesAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    ct);

                await PublishCancellationEventsAsync(
                    affectedAppointments,
                    doctor,
                    createdLeave,
                    ct);

                return result;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync(ct);

                _logger.LogError(
                    exception,
                    "Failed to create doctor leave for doctor {DoctorId} from {StartDate} to {EndDate}. Event type: {EventType}",
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    "DoctorLeaveCreationFailed");

                throw;
            }
        }

        public async Task<IEnumerable<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);
            await GetDoctorAsync(doctorId, ct);

            var leaves = await _doctorLeaveRepository.GetByDoctorIdAsync(
                doctorId,
                ct);

            return _mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves);
        }

        public async Task<DoctorLeaveDto?> GetLeaveForDateAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);
            await GetDoctorAsync(doctorId, ct);

            var leave = await _doctorLeaveRepository.GetLeaveForDateAsync(
                doctorId,
                date,
                ct);

            return leave == null
                ? null
                : _mapper.Map<DoctorLeaveDto>(leave);
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);
            await GetDoctorAsync(doctorId, ct);

            return await _doctorLeaveRepository.IsDoctorOnLeaveAsync(
                doctorId,
                date,
                ct);
        }

        private async Task EnsureLeaveDoesNotOverlapAsync(
            int doctorId,
            DoctorLeaveCreateDto dto,
            CancellationToken ct)
        {
            var hasOverlap = await _doctorLeaveRepository
                .HasOverlappingLeaveAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    ct);

            if (!hasOverlap)
            {
                return;
            }

            _logger.LogWarning(
                "Doctor leave request rejected because doctor {DoctorId} has an overlapping leave from {StartDate} to {EndDate}. Event type: {EventType}",
                doctorId,
                dto.StartDate,
                dto.EndDate,
                "DoctorLeaveOverlapRejected");

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
                    var patientUserId = await _patientRepository
                        .GetPatientUserIdAsync(
                            appointment.PatientId,
                            ct);

                    if (string.IsNullOrWhiteSpace(patientUserId))
                    {
                        _logger.LogWarning(
                            "Doctor-leave notification was not published because patient {PatientId} has no linked user account. Appointment {AppointmentId}, leave {DoctorLeaveId}. Event type: {EventType}",
                            appointment.PatientId,
                            appointment.AppointmentId,
                            leave.DoctorLeaveId,
                            "DoctorLeaveNotificationSkipped");

                        continue;
                    }

                    await _publishEndpoint.Publish(
                        new AppointmentCancelledByDoctorLeaveEvent(
                            appointment.AppointmentId,
                            appointment.PatientId,
                            patientUserId,
                            appointment.Patient?.FullName ?? string.Empty,
                            appointment.DoctorId,
                            doctor.FullName ?? string.Empty,
                            appointment.ScheduledDate.ToDateTime(
                                TimeOnly.MinValue),
                            appointment.TimeSlot ?? string.Empty,
                            leave.StartDate.ToDateTime(TimeOnly.MinValue),
                            leave.EndDate.ToDateTime(TimeOnly.MinValue),
                            leave.Reason),
                        ct);

                    _logger.LogInformation(
                        "Doctor-leave cancellation event published for appointment {AppointmentId}, patient {PatientId}, doctor {DoctorId}, and leave {DoctorLeaveId}. Event type: {EventType}",
                        appointment.AppointmentId,
                        appointment.PatientId,
                        appointment.DoctorId,
                        leave.DoctorLeaveId,
                        "DoctorLeaveCancellationEventPublished");
                }
                catch (OperationCanceledException)
                    when (ct.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Doctor leave {DoctorLeaveId} was created, but the cancellation event could not be published for appointment {AppointmentId} and patient {PatientId}. Event type: {EventType}",
                        leave.DoctorLeaveId,
                        appointment.AppointmentId,
                        appointment.PatientId,
                        "DoctorLeaveCancellationEventFailed");
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

            for (var date = startDate;
                 date <= endDate;
                 date = date.AddDays(1))
            {
                var cacheKey = GetDoctorSlotsCacheKey(doctorId, date);

                try
                {
                    await _cache.RemoveAsync(cacheKey, ct);
                    invalidatedDateCount++;

                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug(
                            "Doctor availability cache invalidated for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                            doctorId,
                            date,
                            "DoctorAvailabilityCacheInvalidated");
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogWarning(
                        exception,
                        "Doctor leave was created, but availability cache invalidation failed for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                        doctorId,
                        date,
                        "DoctorAvailabilityCacheInvalidationFailed");
                }
            }

            _logger.LogInformation(
                "Doctor availability cache invalidated for {InvalidatedDateCount} date(s) for doctor {DoctorId}, from {StartDate} to {EndDate}. Event type: {EventType}",
                invalidatedDateCount,
                doctorId,
                startDate,
                endDate,
                "DoctorLeaveCacheInvalidationCompleted");
        }

        private async Task<Doctor> GetDoctorAsync(
            int doctorId,
            CancellationToken ct)
        {
            var doctor = await _doctorRepository.GetByIdAsync(
                doctorId,
                ct);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid doctor id is required.");
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
                throw new InvalidRequestException(
                    "Leave reason is required.");
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
}
