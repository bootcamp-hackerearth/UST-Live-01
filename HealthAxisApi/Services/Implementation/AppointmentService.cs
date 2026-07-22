using System.Text.Json;

using AutoMapper;

using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.Enums;

using HealthAxisCore_Api.Constants;
using HealthAxisCore_Api.Contracts;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Services.Implementations
{
    public sealed class AppointmentService
        : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        private readonly IDoctorRepository _doctorRepository;

        private readonly IPatientRepository _patientRepository;

        private readonly IDoctorLeaveRepository
            _doctorLeaveRepository;

        private readonly HealthAppDbContext _dbContext;

        private readonly IMapper _mapper;

        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository repository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IDoctorLeaveRepository doctorLeaveRepository,
            HealthAppDbContext dbContext,
            IMapper mapper,
            ILogger<AppointmentService> logger)
        {
            _repository = repository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _doctorLeaveRepository = doctorLeaveRepository;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AppointmentResponseDto>>
            GetAllAsync()
        {
            var appointments =
                await _repository.GetAllAsync();

            return _mapper.Map<
                IEnumerable<AppointmentResponseDto>>(
                    appointments);
        }

        public async Task<
            PagedResponseDto<AppointmentResponseDto>>
            GetPagedAsync(
                int pageNumber,
                int pageSize,
                string? search,
                AppointmentStatus? status,
                DateTime? startDate,
                DateTime? endDate)
        {
            pageNumber =
                NormalizePageNumber(pageNumber);

            pageSize =
                NormalizePageSize(pageSize);

            var appointments =
                await _repository.GetAllAsync();

            var query =
                appointments.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(appointment =>
                    AppointmentMatchesSearch(
                        appointment,
                        search));
            }

            if (status.HasValue)
            {
                query = query.Where(appointment =>
                    appointment.Status ==
                        status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(appointment =>
                    appointment.ScheduledDate.Date >=
                        startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(appointment =>
                    appointment.ScheduledDate.Date <=
                        endDate.Value.Date);
            }

            var totalCount =
                query.Count();

            var totalPages =
                (int)Math.Ceiling(
                    totalCount /
                    (double)pageSize);

            var pagedAppointments =
                query
                    .OrderByDescending(appointment =>
                        appointment.ScheduledDate)
                    .ThenBy(appointment =>
                        FormatAppointmentTimeForSearch(
                            appointment.TimeSlot))
                    .Skip(
                        (pageNumber - 1) *
                        pageSize)
                    .Take(pageSize)
                    .ToList();

            var appointmentDtos =
                _mapper.Map<
                    List<AppointmentResponseDto>>(
                        pagedAppointments);

            return new PagedResponseDto<
                AppointmentResponseDto>
            {
                Items = appointmentDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        public async Task<AppointmentResponseDto?>
            GetByIdAsync(int id)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException(
                    "Appointment not found");
            }

            return _mapper.Map<AppointmentResponseDto>(
                appointment);
        }

        public async Task<AppointmentResponseDto>
            CreateAsync(CreateAppointmentDto dto)
        {
            if (dto == null)
            {
                throw new AppointmentRuleException(
                    "Appointment details are required.");
            }

            ValidateRequiredTimeSlot(dto.TimeSlot);

            var requestedTimeSlot =
                NormalizeTimeSlot(dto.TimeSlot);

            if (string.IsNullOrWhiteSpace(
                requestedTimeSlot))
            {
                throw new AppointmentRuleException(
                    "Invalid time slot format.");
            }

            ValidateAppointmentDateAndTime(
                dto,
                requestedTimeSlot);

            await ValidateDoctorAvailabilityAsync(
                dto);

            await ValidateAppointmentConflictsAsync(
                dto,
                requestedTimeSlot);

            var appointment =
                CreateAppointment(
                    dto,
                    requestedTimeSlot);

            var patient =
                await _patientRepository.GetByIdAsync(
                    appointment.PatientId);

            var patientName =
                patient?.PatientName ??
                $"Patient #{appointment.PatientId}";

            var eventId =
                Guid.NewGuid();

            await SaveAppointmentAndOutboxAsync(
                appointment,
                patientName,
                eventId);

            LogAppointmentAndOutboxCreated(
                appointment,
                eventId);

            return _mapper.Map<
                AppointmentResponseDto>(
                    appointment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists =
                await _repository.Exists(id);

            if (!exists)
            {
                throw new EntityNotFoundException(
                    "Appointment not found");
            }

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<
            IEnumerable<AppointmentResponseDto>>
            GetByDoctorAsync(int doctorId)
        {
            var appointments =
                await _repository.GetByDoctor(
                    doctorId);

            if (appointments == null ||
                !appointments.Any())
            {
                throw new EntityNotFoundException(
                    "No appointments found for this doctor");
            }

            return _mapper.Map<
                IEnumerable<AppointmentResponseDto>>(
                    appointments);
        }

        public async Task<
            IEnumerable<AppointmentResponseDto>>
            GetByPatientAsync(int patientId)
        {
            var appointments =
                await _repository.GetByPatient(
                    patientId);

            if (appointments == null ||
                !appointments.Any())
            {
                throw new EntityNotFoundException(
                    "No appointments found for this patient");
            }

            return _mapper.Map<
                IEnumerable<AppointmentResponseDto>>(
                    appointments);
        }

        public async Task<
            IEnumerable<AppointmentResponseDto>>
            FilterAsync(
                AppointmentStatus? status,
                DateTime? startDate,
                DateTime? endDate)
        {
            var appointments =
                await _repository.FilterAppointments(
                    status,
                    startDate,
                    endDate);

            if (appointments == null ||
                !appointments.Any())
            {
                throw new EntityNotFoundException(
                    "No appointments found for given criteria");
            }

            return _mapper.Map<
                IEnumerable<AppointmentResponseDto>>(
                    appointments);
        }

        public async Task<bool> CancelAsync(
            int id,
            string reason)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException(
                    "Appointment not found");
            }

            if (appointment.Status ==
                    AppointmentStatus.Cancelled ||
                appointment.Status ==
                    AppointmentStatus.DoctorUnavailable)
            {
                throw new AppointmentRuleException(
                    "Appointment already cancelled");
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException(
                    "Cannot cancel a completed appointment");
            }

            await _repository.CancelAppointment(
                id,
                reason);

            return true;
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException(
                    "Appointment not found");
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException(
                    "Cannot confirm a completed appointment");
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new AppointmentRuleException(
                    "Cannot confirm a cancelled appointment");
            }

            if (appointment.Status ==
                AppointmentStatus.DoctorUnavailable)
            {
                throw new AppointmentRuleException(
                    "Cannot confirm an appointment " +
                    "cancelled due to doctor unavailability");
            }

            if (appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                throw new AppointmentRuleException(
                    "Appointment is already confirmed");
            }

            await _repository.ConfirmAppointment(id);

            return true;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var appointment =
                await _repository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException(
                    "Appointment not found");
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new AppointmentRuleException(
                    "Appointment is already completed");
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new AppointmentRuleException(
                    "Cannot complete a cancelled appointment");
            }

            if (appointment.Status ==
                AppointmentStatus.DoctorUnavailable)
            {
                throw new AppointmentRuleException(
                    "Cannot complete an appointment " +
                    "cancelled due to doctor unavailability");
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new AppointmentRuleException(
                    "Only confirmed appointments " +
                    "can be completed");
            }

            await _repository.CompleteAppointment(id);

            return true;
        }

        public async Task<IEnumerable<string>>
            GetBookedSlotsAsync(
                int doctorId,
                DateTime date)
        {
            if (doctorId <= 0)
            {
                throw new AppointmentRuleException(
                    "Doctor ID is required.");
            }

            if (date == default)
            {
                throw new AppointmentRuleException(
                    "Appointment date is required.");
            }

            var bookedSlots =
                await _repository.GetBookedSlotsAsync(
                    doctorId,
                    date);

            return bookedSlots
                .Select(NormalizeTimeSlot)
                .Where(slot =>
                    !string.IsNullOrWhiteSpace(slot))
                .Distinct(
                    StringComparer.OrdinalIgnoreCase)
                .OrderBy(slot => slot)
                .ToList();
        }

        private static int NormalizePageNumber(
            int pageNumber)
        {
            return pageNumber < 1
                ? 1
                : pageNumber;
        }

        private static int NormalizePageSize(
            int pageSize)
        {
            if (pageSize < 1)
            {
                return 10;
            }

            return pageSize > 100
                ? 100
                : pageSize;
        }

        private static bool AppointmentMatchesSearch(
            Appointment appointment,
            string search)
        {
            return
                appointment.AppointmentId
                    .ToString()
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                appointment.PatientId
                    .ToString()
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                appointment.DoctorId
                    .ToString()
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase) ||

                FormatAppointmentTimeForSearch(
                    appointment.TimeSlot)
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateRequiredTimeSlot(
            string? timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new AppointmentRuleException(
                    "Time slot is required.");
            }
        }

        private static void
            ValidateAppointmentDateAndTime(
                CreateAppointmentDto dto,
                string requestedTimeSlot)
        {
            var selectedDateTime =
                BuildAppointmentDateTime(
                    dto.ScheduledDate,
                    requestedTimeSlot);

            if (selectedDateTime <= DateTime.Now)
            {
                throw new AppointmentRuleException(
                    "Previous date or past time slot " +
                    "cannot be booked.");
            }

            var maximumAllowedDate =
                DateTime.Today.AddDays(30);

            if (dto.ScheduledDate.Date >
                maximumAllowedDate)
            {
                throw new AppointmentRuleException(
                    "Appointments can only be booked " +
                    "up to 30 days in advance.");
            }
        }

        private async Task
            ValidateDoctorAvailabilityAsync(
                CreateAppointmentDto dto)
        {
            var isDoctorAvailable =
                await _doctorRepository
                    .IsDoctorAvailable(
                        dto.DoctorId,
                        dto.ScheduledDate);

            if (!isDoctorAvailable)
            {
                throw new AppointmentRuleException(
                    "Doctor not available for " +
                    "the selected date");
            }

            var activeLeave =
                await _doctorLeaveRepository
                    .GetActiveLeaveForDateAsync(
                        dto.DoctorId,
                        dto.ScheduledDate);

            if (activeLeave != null)
            {
                throw new AppointmentRuleException(
                    $"Doctor is unavailable from " +
                    $"{activeLeave.StartDate:dd-MMM-yyyy} " +
                    $"to {activeLeave.EndDate:dd-MMM-yyyy}. " +
                    $"Please choose another doctor.");
            }
        }

        private async Task
            ValidateAppointmentConflictsAsync(
                CreateAppointmentDto dto,
                string requestedTimeSlot)
        {
            var existingAppointments =
                await _repository.GetAllAsync();

            var requestedDate =
                dto.ScheduledDate.Date;

            var activeAppointments =
                existingAppointments
                    .Where(appointment =>
                        IsActiveAppointmentStatus(
                            appointment.Status))
                    .ToList();

            var doctorSlotAlreadyBooked =
                activeAppointments.Any(appointment =>
                    appointment.DoctorId ==
                        dto.DoctorId &&
                    appointment.ScheduledDate.Date ==
                        requestedDate &&
                    NormalizeTimeSlot(
                        appointment.TimeSlot) ==
                        requestedTimeSlot);

            if (doctorSlotAlreadyBooked)
            {
                throw new AppointmentRuleException(
                    "This doctor is already booked for " +
                    "the selected date and time slot. " +
                    "Please choose another slot.");
            }

            var patientAppointmentsForDate =
                activeAppointments
                    .Where(appointment =>
                        appointment.PatientId ==
                            dto.PatientId &&
                        appointment.ScheduledDate.Date ==
                            requestedDate)
                    .ToList();

            ValidatePatientDateConflicts(
                patientAppointmentsForDate,
                dto.DoctorId,
                requestedTimeSlot);
        }

        private static void ValidatePatientDateConflicts(
            IReadOnlyCollection<Appointment>
                patientAppointmentsForDate,
            int doctorId,
            string requestedTimeSlot)
        {
            if (patientAppointmentsForDate.Count == 0)
            {
                return;
            }

            var sameDoctorAppointment =
                patientAppointmentsForDate
                    .FirstOrDefault(appointment =>
                        appointment.DoctorId ==
                            doctorId);

            if (sameDoctorAppointment == null)
            {
                throw new AppointmentRuleException(
                    "You already have an active " +
                    "appointment on this date. " +
                    "Please choose another date.");
            }

            var existingTimeSlot =
                NormalizeTimeSlot(
                    sameDoctorAppointment.TimeSlot);

            if (existingTimeSlot ==
                requestedTimeSlot)
            {
                throw new AppointmentRuleException(
                    "You already have an active " +
                    "appointment with this doctor on " +
                    "the same date and time slot.");
            }

            throw new AppointmentRuleException(
                "You already have an active " +
                "appointment with this doctor on " +
                "the selected date.");
        }

        private Appointment CreateAppointment(
            CreateAppointmentDto dto,
            string requestedTimeSlot)
        {
            var appointment =
                _mapper.Map<Appointment>(dto);

            appointment.ScheduledDate =
                dto.ScheduledDate.Date;

            appointment.TimeSlot =
                requestedTimeSlot;

            appointment.Status =
                AppointmentStatus.Pending;

            appointment.CreatedDate =
                DateTime.UtcNow;

            return appointment;
        }

        private async Task
            SaveAppointmentAndOutboxAsync(
                Appointment appointment,
                string patientName,
                Guid eventId)
        {
            await using var transaction =
                await _dbContext.Database
                    .BeginTransactionAsync();

            try
            {
                /*
                 * The appointment is added directly to the same
                 * DbContext that stores the outbox message.
                 *
                 * Do not call _repository.AddAsync() here unless
                 * that repository method only stages the entity
                 * and never calls SaveChangesAsync independently.
                 */
                await _dbContext.Appointments
                    .AddAsync(appointment);

                /*
                 * Generates the SQL identity AppointmentId.
                 *
                 * This sends the insert to SQL Server but does
                 * not commit the open database transaction.
                 */
                await _dbContext.SaveChangesAsync();

                var appointmentBookedEvent =
                    CreateAppointmentBookedEvent(
                        appointment,
                        patientName,
                        eventId);

                var outboxMessage =
                    CreateOutboxMessage(
                        appointmentBookedEvent);

                await _dbContext.OutboxMessages
                    .AddAsync(outboxMessage);

                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        private static AppointmentBookedEvent
            CreateAppointmentBookedEvent(
                Appointment appointment,
                string patientName,
                Guid eventId)
        {
            return new AppointmentBookedEvent
            {
                EventId =
                    eventId,

                AppointmentId =
                    appointment.AppointmentId,

                PatientId =
                    appointment.PatientId,

                PatientName =
                    patientName,

                DoctorId =
                    appointment.DoctorId,

                ScheduledDate =
                    appointment.ScheduledDate,

                TimeSlot =
                    appointment.TimeSlot
            };
        }

        private static OutboxMessage
            CreateOutboxMessage(
                AppointmentBookedEvent
                    appointmentBookedEvent)
        {
            return new OutboxMessage
            {
                EventId =
                    appointmentBookedEvent.EventId,

                EventType =
                    nameof(AppointmentBookedEvent),

                Payload =
                    JsonSerializer.Serialize(
                        appointmentBookedEvent),

                Status =
                    OutboxMessageStatuses.Pending,

                RetryCount = 0,

                CreatedDate =
                    DateTime.UtcNow,

                PublishedDate = null,
                LastAttemptDate = null,
                NextRetryDate = null,
                ErrorMessage = null
            };
        }

        private void LogAppointmentAndOutboxCreated(
            Appointment appointment,
            Guid eventId)
        {
            if (!_logger.IsEnabled(
                LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Appointment saved with pending " +
                "outbox event. " +
                "EventId: {EventId}, " +
                "AppointmentId: {AppointmentId}, " +
                "PatientId: {PatientId}, " +
                "DoctorId: {DoctorId}, " +
                "ScheduledDate: {ScheduledDate}, " +
                "TimeSlot: {TimeSlot}",
                eventId,
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.DoctorId,
                appointment.ScheduledDate,
                appointment.TimeSlot);
        }

        private static bool IsActiveAppointmentStatus(
            AppointmentStatus status)
        {
            return
                status ==
                    AppointmentStatus.Pending ||
                status ==
                    AppointmentStatus.Confirmed;
        }

        private static DateTime
            BuildAppointmentDateTime(
                DateTime scheduledDate,
                string timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                throw new AppointmentRuleException(
                    "Time slot is required.");
            }

            if (TimeSpan.TryParse(
                timeSlot,
                out var parsedTime))
            {
                return scheduledDate.Date.Add(
                    parsedTime);
            }

            if (DateTime.TryParse(
                timeSlot,
                out var parsedDateTime))
            {
                return scheduledDate.Date.Add(
                    parsedDateTime.TimeOfDay);
            }

            throw new AppointmentRuleException(
                "Invalid time slot format.");
        }

        private static string NormalizeTimeSlot(
            object? timeSlot)
        {
            if (timeSlot == null)
            {
                return string.Empty;
            }

            if (timeSlot is TimeOnly timeOnly)
            {
                return timeOnly.ToString("HH:mm");
            }

            if (timeSlot is TimeSpan timeSpan)
            {
                return timeSpan.ToString(
                    @"hh\:mm");
            }

            if (timeSlot is DateTime dateTime)
            {
                return dateTime.ToString("HH:mm");
            }

            var value =
                timeSlot.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            if (value.Contains(
                '-',
                StringComparison.Ordinal))
            {
                value = value
                    .Split('-')[0]
                    .Trim();
            }

            if (TimeSpan.TryParse(
                value,
                out var parsedTimeSpan))
            {
                return parsedTimeSpan.ToString(
                    @"hh\:mm");
            }

            if (DateTime.TryParse(
                value,
                out var parsedDateTime))
            {
                return parsedDateTime.ToString(
                    "HH:mm");
            }

            return value.Trim();
        }

        private static string
            FormatAppointmentTimeForSearch(
                object? timeSlot)
        {
            if (timeSlot == null)
            {
                return string.Empty;
            }

            if (timeSlot is TimeOnly timeOnly)
            {
                return timeOnly.ToString(
                    "hh:mm tt");
            }

            if (timeSlot is TimeSpan timeSpan)
            {
                return DateTime.Today
                    .Add(timeSpan)
                    .ToString("hh:mm tt");
            }

            if (timeSlot is DateTime dateTime)
            {
                return dateTime.ToString(
                    "hh:mm tt");
            }

            return timeSlot.ToString() ??
                string.Empty;
        }
    }
}
