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
                throw new InvalidRequestException("Valid appointment id is required.");
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new EntityNotFoundException("Appointment", id);
            }

            await LoadAppointmentNavigationDataAsync(appointment);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> BookAppointmentAsync(AppointmentCreateDto dto)
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

            DateOnly scheduledDate = DateOnly.FromDateTime(dto.ScheduledDate);

            if (scheduledDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException("Past date is not allowed.");
            }

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", dto.PatientId);
            }

            var patientUserId = await _patientRepository.GetPatientUserIdAsync(dto.PatientId);

            if (string.IsNullOrWhiteSpace(patientUserId))
            {
                throw new BusinessRuleViolationException(
                    "Patient user account is not linked.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", dto.DoctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleViolationException("Selected doctor is inactive.");
            }

            var doctorLeave = await _doctorLeaveRepository.GetLeaveForDateAsync(
                dto.DoctorId,
                scheduledDate);

            if (doctorLeave != null)
            {
                throw new BusinessRuleViolationException(
                    $"The selected doctor is on leave from " +
                    $"{doctorLeave.StartDate:dd MMM yyyy} to " +
                    $"{doctorLeave.EndDate:dd MMM yyyy}. " +
                    "Please select another appointment date.");
            }

            string slot = dto.TimeSlot.Trim();

            bool sameDoctorSameDay = await _appointmentRepository
                .HasAppointmentWithDoctorOnSameDayAsync(
                    dto.PatientId,
                    dto.DoctorId,
                    scheduledDate);

            if (sameDoctorSameDay)
            {
                throw new BusinessRuleViolationException(
                    "Patient already has an appointment with this doctor on the same day.");
            }

            bool patientSlotConflict = await _appointmentRepository
                .HasPatientSlotConflictAsync(
                    dto.PatientId,
                    scheduledDate,
                    slot);

            if (patientSlotConflict)
            {
                throw new BusinessRuleViolationException(
                    "Patient already has an appointment in this time slot.");
            }

            bool doctorSlotBooked = await _appointmentRepository
                .IsDoctorSlotBookedAsync(
                    dto.DoctorId,
                    scheduledDate,
                    slot);

            if (doctorSlotBooked)
            {
                throw new BusinessRuleViolationException(
                    "Doctor is already booked for this time slot.");
            }

            var appointment = _mapper.Map<Appointment>(dto);

            appointment.PatientId = dto.PatientId;
            appointment.DoctorId = dto.DoctorId;
            appointment.ScheduledDate = scheduledDate;
            appointment.TimeSlot = slot;
            appointment.Status = AppointmentStatus.Pending;
            appointment.CancellationReason = null;

            var createdAppointment = await _appointmentRepository.Add(appointment);

            createdAppointment.Patient = patient;
            createdAppointment.Doctor = doctor;

            await RemoveDoctorSlotsCacheAsync(
                createdAppointment.DoctorId,
                createdAppointment.ScheduledDate);

            var appointmentBookedEvent = new AppointmentBookedEvent(
                createdAppointment.AppointmentId,
                createdAppointment.PatientId,
                patientUserId,
                patient.FullName ?? string.Empty,
                createdAppointment.DoctorId,
                doctor.FullName ?? string.Empty,
                createdAppointment.ScheduledDate.ToDateTime(TimeOnly.MinValue),
                createdAppointment.TimeSlot ?? string.Empty);

            await _publishEndpoint.Publish(appointmentBookedEvent);

            return _mapper.Map<AppointmentDto>(createdAppointment);
        }

        public async Task UpdateAppointmentStatusAsync(
            int id,
            AppointmentStatus status,
            string? cancellationReason = null)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid appointment id is required.");
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
                throw new InvalidRequestException("Cancellation reason is required.");
            }

            appointment.Status = status;

            appointment.CancellationReason =
                status == AppointmentStatus.Cancelled
                    ? cancellationReason?.Trim()
                    : null;

            await _appointmentRepository.Update(id, appointment);

            await RemoveDoctorSlotsCacheAsync(
                appointment.DoctorId,
                appointment.ScheduledDate);
        }

        public async Task<IEnumerable<string>> GetAvailableSlotsAsync(
            int doctorId,
            DateOnly date)
        {
            if (doctorId <= 0)
            {
                throw new InvalidRequestException("Valid doctor id is required.");
            }

            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException("Past date is not allowed.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleViolationException("Doctor is inactive.");
            }

            var cacheKey = GetDoctorSlotsCacheKey(
                doctorId,
                date);

            var cachedSlotsJson = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedSlotsJson))
            {
                var cachedSlots = JsonSerializer.Deserialize<List<string>>(
                    cachedSlotsJson);

                if (cachedSlots != null)
                {

                    if (_logger.IsEnabled(LogLevel.Information))
                    {

                        _logger.LogInformation(
                        "\n" +
                        "================ DOCTOR SLOTS CACHE HIT ================\n" +
                        " Doctor Id : {DoctorId}\n" +
                        " Date      : {Date}\n" +
                        " Cache Key : {CacheKey}\n" +
                        " Count     : {Count}\n" +
                        "========================================================",
                        doctorId,
                        date,
                        cacheKey,
                        cachedSlots.Count);
                    }

                    return cachedSlots;
                }
            }


            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                    "\n" +
                    "================ DOCTOR SLOTS CACHE MISS ===============\n" +
                    " Doctor Id : {DoctorId}\n" +
                    " Date      : {Date}\n" +
                    " Cache Key : {CacheKey}\n" +
                    " Action    : Loading available slots from database\n" +
                    "========================================================",
                    doctorId,
                    date,
                    cacheKey);
            }


            var availableSlots = new List<string>();

            foreach (var slot in TimeSlots.Slots)
            {
                bool isBooked = await _appointmentRepository.IsDoctorSlotBookedAsync(
                    doctorId,
                    date,
                    slot);

                if (!isBooked)
                {
                    availableSlots.Add(slot);
                }
            }

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(availableSlots),
                cacheOptions);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                "\n" +
                "================ DOCTOR SLOTS CACHE SET ================\n" +
                " Doctor Id           : {DoctorId}\n" +
                " Date                : {Date}\n" +
                " Cache Key           : {CacheKey}\n" +
                " Available Slot Count: {Count}\n" +
                " Absolute Expiry     : 5 minute(s)\n" +
                " Sliding Expiry      : 2 minute(s)\n" +
                "========================================================",
                doctorId,
                date,
                cacheKey,
                availableSlots.Count);
            }
            return availableSlots;
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid appointment id is required.");
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

            bool deleted = await _appointmentRepository.DeleteAsync(id);

            if (!deleted)
            {
                throw new BusinessRuleViolationException(
                    "Unable to delete appointment.");
            }

            await RemoveDoctorSlotsCacheAsync(
                appointment.DoctorId,
                appointment.ScheduledDate);
        }

        private async Task LoadAppointmentNavigationDataAsync(Appointment appointment)
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
            var cacheKey = GetDoctorSlotsCacheKey(
                doctorId,
                date);

            await _cache.RemoveAsync(cacheKey);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(
                "\n" +
                "================ DOCTOR SLOTS CACHE INVALIDATED ================\n" +
                " Doctor Id : {DoctorId}\n" +
                " Date      : {Date}\n" +
                " Cache Key : {CacheKey}\n" +
                " Reason    : Appointment data changed\n" +
                "================================================================",
                doctorId,
                date,
                cacheKey);
            }
        }
    }
}