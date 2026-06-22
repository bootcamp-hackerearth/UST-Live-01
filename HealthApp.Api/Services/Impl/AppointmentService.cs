using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Constants;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;

namespace HealthApp.Api.Services.Impl
{
    public class AppointmentService : IAppointmentService
    {

        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsAsync(
            int? doctorId = null,
            int? patientId = null,
            bool onlyUpcoming = false)
        {
            var appointments = await _appointmentRepository.GetAppointmentsAsync(
                doctorId,
                patientId,
                onlyUpcoming);

            foreach (var appointment in appointments)
            {
                await LoadAppointmentNavigationDataAsync(appointment);
            }

            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
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

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", dto.DoctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleViolationException("Selected doctor is inactive.");
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

            return availableSlots;
        }

        private async Task LoadAppointmentNavigationDataAsync(Appointment appointment)
        {
            appointment.Patient ??= await _patientRepository.GetByIdAsync(
                appointment.PatientId);

            appointment.Doctor ??= await _doctorRepository.GetByIdAsync(
                appointment.DoctorId);
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
        }
    }
}
