using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using HealthApp.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthApp.Api.Services.Impl
{
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
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IDoctorLeaveRepository doctorLeaveRepository,
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            IDistributedCache cache,
            ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _doctorLeaveRepository = doctorLeaveRepository;
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

            if (filter.FromDate.HasValue &&
                filter.ToDate.HasValue &&
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

            var appointmentDtos =
                _mapper.Map<List<AppointmentDto>>(appointments);

            return new PagedResultDto<AppointmentDto>
            {
                Items = appointmentDtos,
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

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", id);
            }

            await LoadAppointmentNavigationDataAsync(appointment);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> BookAppointmentAsync(
            AppointmentCreateDto dto)
        {
            ValidateBookingRequest(dto);

            var scheduledDate = DateOnly.FromDateTime(dto.ScheduledDate);
            ValidateScheduledDate(scheduledDate);

            var patient = await GetPatientForBookingAsync(dto.PatientId);
            var patientUserId = await GetPatientUserIdForBookingAsync(
                dto.PatientId);
            var doctor = await GetDoctorForBookingAsync(
                dto.DoctorId,
                dto.PatientId);

            await EnsureDoctorIsNotOnLeaveAsync(
                dto,
                scheduledDate);

            var slot = dto.TimeSlot.Trim();

            await EnsureNoBookingConflictsAsync(
                dto.PatientId,
                dto.DoctorId,
                scheduledDate,
                slot);

            var createdAppointment = await CreateAppointmentAsync(
                dto,
                scheduledDate,
                slot,
                patient,
                doctor);

            await PublishAppointmentBookedEventAsync(
                createdAppointment,
                patientUserId,
                patient,
                doctor);

            LogAppointmentBooked(createdAppointment);

            return _mapper.Map<AppointmentDto>(createdAppointment);
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

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", id);
            }

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

            var cacheKey = GetDoctorSlotsCacheKey(doctorId, date);
            var cachedAvailability = await GetCachedAvailabilityAsync(
                cacheKey,
                doctorId,
                date);

            if (cachedAvailability != null)
            {
                return cachedAvailability;
            }

            var availability = await BuildDoctorAvailabilityAsync(
                doctorId,
                date);

            await CacheDoctorAvailabilityAsync(
                cacheKey,
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

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", id);
            }

            if (appointment.Status != AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleViolationException(
                    "Only cancelled appointments can be deleted.");
            }

            var deleted = await _appointmentRepository.DeleteAsync(id);

            if (!deleted)
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

        private static void ValidateAvailabilityRequest(
            int doctorId,
            DateOnly date)
        {
            if (doctorId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid doctor id is required.");
            }

            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException(
                    "Past date is not allowed.");
            }
        }

        private async Task EnsureDoctorIsAvailableForLookupAsync(int doctorId)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleViolationException(
                    "Doctor is inactive.");
            }
        }

        private async Task<DoctorAvailabilityDto?> GetCachedAvailabilityAsync(
            string cacheKey,
            int doctorId,
            DateOnly date)
        {
            var cachedAvailabilityJson = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(cachedAvailabilityJson))
            {
                return null;
            }

            try
            {
                var cachedAvailability =
                    JsonSerializer.Deserialize<DoctorAvailabilityDto>(
                        cachedAvailabilityJson);

                if (cachedAvailability != null)
                {
                    LogAvailabilityCacheHit(doctorId, date);
                }

                return cachedAvailability;
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
                Message =
                    $"Doctor is on leave from " +
                    $"{leave.StartDate:dd MMM yyyy} to " +
                    $"{leave.EndDate:dd MMM yyyy}.",
                Slots = TimeSlots.Slots
                    .Select(slot => new DoctorAvailabilitySlotDto
                    {
                        TimeSlot = slot,
                        IsAvailable = false,
                        Status = "DoctorOnLeave"
                    })
                    .ToList()
            };
        }

        private async Task<DoctorAvailabilityDto>
            CreateRegularAvailabilityAsync(
                int doctorId,
                DateOnly date)
        {
            var slots = new List<DoctorAvailabilitySlotDto>();

            foreach (var slot in TimeSlots.Slots)
            {
                var isBooked = await _appointmentRepository
                    .IsDoctorSlotBookedAsync(
                        doctorId,
                        date,
                        slot);

                slots.Add(new DoctorAvailabilitySlotDto
                {
                    TimeSlot = slot,
                    IsAvailable = !isBooked,
                    Status = isBooked ? "Booked" : "Available"
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
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            var availabilityJson = JsonSerializer.Serialize(availability);

            await _cache.SetStringAsync(
                cacheKey,
                availabilityJson,
                cacheOptions);

            LogAvailabilityCached(
                doctorId,
                date,
                availability.IsDoctorOnLeave);
        }

        private void LogAvailabilityCacheHit(
            int doctorId,
            DateOnly date)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            _logger.LogDebug(
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
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                exception,
                "Invalid cached doctor availability was removed for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                doctorId,
                date,
                "DoctorAvailabilityCacheInvalid");
        }

        private void LogAvailabilityCached(
            int doctorId,
            DateOnly date,
            bool isDoctorOnLeave)
        {
            if (!_logger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

            _logger.LogDebug(
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
                throw new InvalidRequestException(
                    "Appointment data is required.");
            }

            if (dto.PatientId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid patient is required.");
            }

            if (dto.DoctorId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid doctor is required.");
            }

            if (dto.ScheduledDate == default)
            {
                throw new InvalidRequestException(
                    "Scheduled date is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.TimeSlot))
            {
                throw new InvalidRequestException(
                    "Time slot is required.");
            }
        }

        private static void ValidateScheduledDate(DateOnly scheduledDate)
        {
            if (scheduledDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException(
                    "Past date is not allowed.");
            }
        }

        private async Task<Patient> GetPatientForBookingAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            return patient ?? throw new EntityNotFoundException(
                "Patient",
                patientId);
        }

        private async Task<string> GetPatientUserIdForBookingAsync(
            int patientId)
        {
            var patientUserId = await _patientRepository
                .GetPatientUserIdAsync(
                    patientId,
                    CancellationToken.None);

            if (!string.IsNullOrWhiteSpace(patientUserId))
            {
                return patientUserId;
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
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

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

            throw new BusinessRuleViolationException(
                "Selected doctor is inactive.");
        }

        private async Task EnsureDoctorIsNotOnLeaveAsync(
            AppointmentCreateDto dto,
            DateOnly scheduledDate)
        {
            var doctorLeave = await _doctorLeaveRepository
                .GetLeaveForDateAsync(dto.DoctorId, scheduledDate);

            if (doctorLeave == null)
            {
                return;
            }

            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(
                    "Appointment booking was rejected because doctor {DoctorId} is on leave on {ScheduledDate}. Patient {PatientId}, leave {DoctorLeaveId}. Event type: {EventType}",
                    dto.DoctorId,
                    scheduledDate,
                    dto.PatientId,
                    doctorLeave.DoctorLeaveId,
                    "AppointmentBookingRejectedDoctorOnLeave");
            }

            throw new BusinessRuleViolationException(
                $"The selected doctor is on leave from " +
                $"{doctorLeave.StartDate:dd MMM yyyy} to " +
                $"{doctorLeave.EndDate:dd MMM yyyy}. " +
                "Please select another appointment date.");
        }

        private async Task EnsureNoBookingConflictsAsync(
            int patientId,
            int doctorId,
            DateOnly scheduledDate,
            string slot)
        {
            var sameDoctorSameDay = await _appointmentRepository
                .HasAppointmentWithDoctorOnSameDayAsync(
                    patientId,
                    doctorId,
                    scheduledDate);

            if (sameDoctorSameDay)
            {
                throw new BusinessRuleViolationException(
                    "Patient already has an appointment with this doctor on the same day.");
            }

            var patientSlotConflict = await _appointmentRepository
                .HasPatientSlotConflictAsync(
                    patientId,
                    scheduledDate,
                    slot);

            if (patientSlotConflict)
            {
                throw new BusinessRuleViolationException(
                    "Patient already has an appointment in this time slot.");
            }

            var doctorSlotBooked = await _appointmentRepository
                .IsDoctorSlotBookedAsync(
                    doctorId,
                    scheduledDate,
                    slot);

            if (doctorSlotBooked)
            {
                throw new BusinessRuleViolationException(
                    "Doctor is already booked for this time slot.");
            }
        }

        private async Task<Appointment> CreateAppointmentAsync(
            AppointmentCreateDto dto,
            DateOnly scheduledDate,
            string slot,
            Patient patient,
            Doctor doctor)
        {
            var appointment = _mapper.Map<Appointment>(dto);

            appointment.PatientId = dto.PatientId;
            appointment.DoctorId = dto.DoctorId;
            appointment.ScheduledDate = scheduledDate;
            appointment.TimeSlot = slot;
            appointment.Status = AppointmentStatus.Pending;
            appointment.CancellationReason = null;

            var createdAppointment = await _appointmentRepository.Add(
                appointment);

            createdAppointment.Patient = patient;
            createdAppointment.Doctor = doctor;

            await RemoveDoctorSlotsCacheAsync(
                createdAppointment.DoctorId,
                createdAppointment.ScheduledDate);

            return createdAppointment;
        }

        private async Task PublishAppointmentBookedEventAsync(
            Appointment createdAppointment,
            string patientUserId,
            Patient patient,
            Doctor doctor)
        {
            var appointmentBookedEvent = new AppointmentBookedEvent(
                createdAppointment.AppointmentId,
                createdAppointment.PatientId,
                patientUserId,
                patient.FullName ?? string.Empty,
                createdAppointment.DoctorId,
                doctor.FullName ?? string.Empty,
                createdAppointment.ScheduledDate.ToDateTime(
                    TimeOnly.MinValue),
                createdAppointment.TimeSlot ?? string.Empty);

            await _publishEndpoint.Publish(appointmentBookedEvent);
        }

        private void LogAppointmentBooked(Appointment appointment)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Appointment {AppointmentId} booked for patient {PatientId} with doctor {DoctorId} on {ScheduledDate}. Event type: {EventType}",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ScheduledDate,
                "AppointmentBooked");
        }

        private async Task LoadAppointmentNavigationDataAsync(
            Appointment appointment)
        {
            appointment.Patient ??= await _patientRepository.GetByIdAsync(
                appointment.PatientId);

            appointment.Doctor ??= await _doctorRepository.GetByIdAsync(
                appointment.DoctorId);
        }

        private static string GetDoctorSlotsCacheKey(
            int doctorId,
            DateOnly date)
        {
            return $"doctor:{doctorId}:slots:{date:yyyy-MM-dd}";
        }

        private async Task RemoveDoctorSlotsCacheAsync(
            int doctorId,
            DateOnly date)
        {
            var cacheKey = GetDoctorSlotsCacheKey(doctorId, date);

            await _cache.RemoveAsync(cacheKey);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(
                    "Doctor availability cache invalidated for doctor {DoctorId} on {AvailabilityDate}. Event type: {EventType}",
                    doctorId,
                    date,
                    "DoctorAvailabilityCacheInvalidated");
            }
        }
    }
}