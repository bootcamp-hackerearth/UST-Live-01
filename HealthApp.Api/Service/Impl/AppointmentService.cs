using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Messaging.Events;
using HealthApp.Api.Messaging.Publisher;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthApp.Api.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IAppointmentEventPublisher _appointmentEventPublisher;
        private readonly IDoctorLeaveRepository _doctorLeaveRepository;
        private readonly INotificationService _notificationService;
        private readonly IDistributedCache _cache;

        private const string AppointmentEntity = "Appointment";

        private const string DoctorAvailabilityCachePrefix =
            "appointment:doctor:availability";

        private readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        public AppointmentService(
            IAppointmentRepository repo,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            INotificationService notificationService,
            IAppointmentEventPublisher appointmentEventPublisher,
            IDoctorLeaveRepository doctorLeaveRepository,
            IDistributedCache cache)
        {
            _repo = repo;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _notificationService = notificationService;
            _appointmentEventPublisher = appointmentEventPublisher;
            _doctorLeaveRepository = doctorLeaveRepository;
            _cache = cache;
        }

        public async Task<object> Add(AppointmentDto dto, string identityUserId)
        {
            var patient = await _patientRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new EntityNotFoundException("Patient", 0);

            var isDoctorOnLeave = await _doctorLeaveRepository
                .IsDoctorOnLeaveAsync(dto.DoctorId, dto.ScheduledDate);

            if (isDoctorOnLeave)
                throw new AppointmentRuleException(
                    "Doctor is on leave for the selected date. Please choose another date.");

            var isBooked = await _repo.IsSlotBookedAsync(
                dto.DoctorId,
                dto.ScheduledDate,
                dto.TimeSlot);

            if (isBooked)
                throw new AppointmentRuleException("Slot already booked");

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = patient.PatientId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = "Pending"
            };

            await _repo.addAsync(appointment);

            await RemoveDoctorAvailabilityCacheAsync(
                appointment.DoctorId,
                appointment.ScheduledDate);

            var doctor = await _doctorRepository.getbyidAsync(dto.DoctorId);

            var appointmentBookEvent = new AppointmentBookEvent
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = patient.FullName!,
                DoctorName = doctor?.FullName ?? "Doctor",
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot
            };

            await _appointmentEventPublisher
                .PublishAppointmentBookedAsync(appointmentBookEvent);

            return new
            {
                message = "Appointment booked successfully"
            };
        }

        public async Task<AppointmentDto> GetAppointmentById(int appointmentId)
        {
            var appointment = await _repo.getbyidAsync(appointmentId);

            if (appointment == null)
                throw new EntityNotFoundException(
                    AppointmentEntity,
                    appointmentId);

            await LoadNavigation(appointment);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> CancelAppointment(
            int appointmentId,
            string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new AppointmentRuleException("Reason required");

            var saved = await _repo.CancelAppointmentAsync(
                appointmentId,
                reason);

            if (saved == null)
                throw new EntityNotFoundException(
                    AppointmentEntity,
                    appointmentId);

            await RemoveDoctorAvailabilityCacheAsync(
                saved.DoctorId,
                saved.ScheduledDate);

            await SendAppointmentCancelledNotification(saved, reason);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto> ConfirmAppointment(int appointmentId)
        {
            var saved = await _repo.UpdateStatusAsync(
                appointmentId,
                "Confirmed");

            if (saved == null)
                throw new EntityNotFoundException(
                    AppointmentEntity,
                    appointmentId);

            await RemoveDoctorAvailabilityCacheAsync(
                saved.DoctorId,
                saved.ScheduledDate);

            await SendAppointmentConfirmedNotification(saved);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto> CompleteAppointment(int appointmentId)
        {
            var saved = await _repo.UpdateStatusAsync(
                appointmentId,
                "Completed");

            if (saved == null)
                throw new EntityNotFoundException(
                    AppointmentEntity,
                    appointmentId);

            await RemoveDoctorAvailabilityCacheAsync(
                saved.DoctorId,
                saved.ScheduledDate);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<DoctorAvailabilityResponseDto> CheckDoctorAvailability(
            int doctorId,
            DateTime date)
        {
            var selectedDate = date.Date;

            var cacheKey = GetDoctorAvailabilityCacheKey(doctorId,selectedDate);

            var cachedValue = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrWhiteSpace(cachedValue))
            {
                var cachedResponse =
                    JsonSerializer.Deserialize<DoctorAvailabilityResponseDto>(cachedValue);

                if (cachedResponse != null)
                {
                    return cachedResponse;
                }

                await _cache.RemoveAsync(cacheKey);
            }

            var allSlots = new List<string>
            {
                "09:00 AM",
                "10:00 AM",
                "11:00 AM",
                "12:00 PM",
                "01:00 PM",
                "02:00 PM",
                "03:00 PM",
                "04:00 PM",
                "05:00 PM"
            };

            var isDoctorOnLeave = await _doctorLeaveRepository
                .IsDoctorOnLeaveAsync(doctorId, selectedDate);

            DoctorAvailabilityResponseDto response;

            if (isDoctorOnLeave)
            {
                response = new DoctorAvailabilityResponseDto
                {
                    DoctorId = doctorId,
                    Date = selectedDate,
                    IsDoctorOnLeave = true,
                    Message = "Doctor is on leave for the selected date.",
                    Slots = allSlots.Select(slot => new DoctorSlotDto
                    {
                        TimeSlot = slot,
                        IsAvailable = false,
                        Status = "Doctor On Leave"
                    }).ToList()
                };
            }
            else
            {
                var bookedSlots = await _repo.GetBookedSlotsAsync(
                    doctorId,
                    selectedDate);

                bookedSlots ??= new List<string>();

                response = new DoctorAvailabilityResponseDto
                {
                    DoctorId = doctorId,
                    Date = selectedDate,
                    IsDoctorOnLeave = false,
                    Message = "Doctor is available for the selected date.",
                    Slots = allSlots.Select(slot => new DoctorSlotDto
                    {
                        TimeSlot = slot,
                        IsAvailable = !bookedSlots.Contains(slot),
                        Status = bookedSlots.Contains(slot)
                            ? "Booked"
                            : "Available"
                    }).ToList()
                };
            }

            var serializedResponse = JsonSerializer.Serialize(response);

            await _cache.SetStringAsync(
                cacheKey,
                serializedResponse,
                cacheOptions);

            return response;
        }

        public async Task<bool> IsSlotBooked(
            int doctorId,
            DateTime date,
            string timeSlot)
        {
            return await _repo.IsSlotBookedAsync(
                doctorId,
                date,
                timeSlot);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId,
            DateTime fromDate,
            DateTime toDate)
        {
            var list = await _repo.GetUpcomingByDoctorAsync(
                doctorId,
                fromDate,
                toDate);

            return _mapper.Map<List<AppointmentDto>>(
                list ?? new List<Appointment>());
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByUserAsync(
            string identityUserId)
        {
            var patient = await _patientRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new EntityNotFoundException("Patient", 0);

            var list = await _repo.GetByPatientIdAsync(patient.PatientId);

            return _mapper.Map<List<AppointmentDto>>(
                list ?? new List<Appointment>());
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorAsync(
            string identityUserId)
        {
            var doctor = await _doctorRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor", 0);

            var list = await _repo.GetByDoctorIdAsync(doctor.DoctorId);

            return _mapper.Map<List<AppointmentDto>>(
                list ?? new List<Appointment>());
        }

        public async Task<(List<AppointmentDto> Items, int TotalCount)>
            GetPagedAppointments(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new AppointmentRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new AppointmentRuleException("Invalid page size");

            var (items, total) = await _repo.GetPagedAppointmentsAsync(
                pageNumber,
                pageSize);

            return (_mapper.Map<List<AppointmentDto>>(items), total);
        }

        public async Task<(List<AppointmentDto> Items, int TotalCount)>
            GetAppointmentsByPatientAndDoctorPaged(
                int? patientId,
                int? doctorId,
                int pageNumber,
                int pageSize)
        {
            if (!patientId.HasValue && !doctorId.HasValue)
                throw new AppointmentRuleException("Provide patient or doctor");

            if (pageNumber <= 0)
                throw new AppointmentRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new AppointmentRuleException("Invalid page size");

            var (items, total) = await _repo.GetByPatientAndDoctor(
                patientId,
                doctorId,
                pageNumber,
                pageSize);

            return (_mapper.Map<List<AppointmentDto>>(items), total);
        }

        private async Task LoadNavigation(Appointment appointment)
        {
            if (appointment.Patient == null)
            {
                appointment.Patient =
                    await _patientRepository.getbyidAsync(
                        appointment.PatientId);
            }

            if (appointment.Doctor == null)
            {
                appointment.Doctor =
                    await _doctorRepository.getbyidAsync(
                        appointment.DoctorId);
            }
        }

        private async Task SendAppointmentConfirmedNotification(
            Appointment appointment)
        {
            var patient = await _patientRepository
                .getbyidAsync(appointment.PatientId);

            var doctor = await _doctorRepository
                .getbyidAsync(appointment.DoctorId);

            if (patient == null ||
                string.IsNullOrWhiteSpace(patient.IdentityUserId))
            {
                return;
            }

            await _notificationService.CreateAsync(new NotificationCreateDto
            {
                UserId = patient.IdentityUserId,
                Title = "Appointment Confirmed",
                Message =
                    $"Your appointment with Dr. {doctor?.FullName ?? "Doctor"} " +
                    $"on {appointment.ScheduledDate:dd-MM-yyyy} " +
                    $"at {appointment.TimeSlot} has been confirmed.",
                EventType = "AppointmentConfirmed",
                CreatedAt = DateTime.UtcNow
            });
        }

        private async Task SendAppointmentCancelledNotification(
            Appointment appointment,
            string reason)
        {
            var patient = await _patientRepository
                .getbyidAsync(appointment.PatientId);

            var doctor = await _doctorRepository
                .getbyidAsync(appointment.DoctorId);

            if (patient == null ||
                string.IsNullOrWhiteSpace(patient.IdentityUserId))
            {
                return;
            }

            await _notificationService.CreateAsync(new NotificationCreateDto
            {
                UserId = patient.IdentityUserId,
                Title = "Appointment Cancelled",
                Message =
                    $"Your appointment with Dr. {doctor?.FullName ?? "Doctor"} " +
                    $"on {appointment.ScheduledDate:dd-MM-yyyy} " +
                    $"at {appointment.TimeSlot} has been cancelled. " +
                    $"Reason: {reason}",
                EventType = "AppointmentCancelled",
                CreatedAt = DateTime.UtcNow
            });
        }

        private static string GetDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"{DoctorAvailabilityCachePrefix}:{doctorId}:{date.Date:yyyyMMdd}";
        }

        private async Task RemoveDoctorAvailabilityCacheAsync(
            int doctorId,
            DateTime date)
        {
            var cacheKey = GetDoctorAvailabilityCacheKey(
                doctorId,
                date);

            await _cache.RemoveAsync(cacheKey);
        }
    }
}