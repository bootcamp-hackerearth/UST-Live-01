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


        private const string AppointmentEntity = "Appointment";

        public AppointmentService(
            IAppointmentRepository repo,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            IAppointmentEventPublisher appointmentEventPublisher,
            IDoctorLeaveRepository doctorLeaveRepository)
        {
            _repo = repo;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _appointmentEventPublisher = appointmentEventPublisher;
            _doctorLeaveRepository = doctorLeaveRepository;
        }
        private const string DoctorAvailabilityCachePrefix = "appointment:doctor:availability";

        private readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
        };



        public async Task<object> Add(AppointmentDto dto, string identityUserId)
        {
            var patient = await _patientRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new EntityNotFoundException("Patient", 0);



            var isDoctorOnLeave = await _doctorLeaveRepository.
                IsDoctorOnLeaveAsync(dto.DoctorId, dto.ScheduledDate);

            if (isDoctorOnLeave)
                throw new AppointmentRuleException
                    ("Doctor is on leave for the selected date. Please choose another date.");

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

            var doctor = await _doctorRepository.getbyidAsync(dto.DoctorId);

            var appointmentBookEvent = new AppointmentBookEvent
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = patient.FullName!,
                DoctorName = doctor?.FullName ?? "Doctor",
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot
            };

            await _appointmentEventPublisher.PublishAppointmentBookedAsync(appointmentBookEvent);

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

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<DoctorAvailabilityResponseDto> CheckDoctorAvailability(
    int doctorId,
    DateTime date)
        {
            var selectedDate = date.Date;

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

            if (isDoctorOnLeave)
            {
                return new DoctorAvailabilityResponseDto
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

            var bookedSlots = await _repo.GetBookedSlotsAsync(
                doctorId,
                selectedDate);

            bookedSlots ??= new List<string>();

            return new DoctorAvailabilityResponseDto
            {
                DoctorId = doctorId,
                Date = selectedDate,
                IsDoctorOnLeave = false,
                Message = "Doctor is available for the selected date.",
                Slots = allSlots.Select(slot => new DoctorSlotDto
                {
                    TimeSlot = slot,
                    IsAvailable = !bookedSlots.Contains(slot),
                    Status = bookedSlots.Contains(slot) ? "Booked" : "Available"
                }).ToList()
            };
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
    }
}