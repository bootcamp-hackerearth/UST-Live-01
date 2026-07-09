using AutoMapper;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using CustomValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint? _publishEndpoint;
        private readonly ILogger<AppointmentService>? _logger;
        private readonly IDistributedCache? _cache;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Patient> patientRepository,
            IMapper mapper,
            IPublishEndpoint? publishEndpoint = null,
            ILogger<AppointmentService>? logger = null,
            IDistributedCache? cache = null)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> AddAsync(CreateAppointmentDto dto)
        {
            if (dto.ScheduledDate.Date < DateTime.Today)
            {
                throw new CustomValidationException("Appointments cannot be booked for past dates.");
            }

            if (dto.ScheduledDate.Date > DateTime.Today.AddMonths(6))
            {
                throw new CustomValidationException("Appointments can only be booked up to 6 months in advance.");
            }

            if (string.IsNullOrWhiteSpace(dto.TimeSlot))
            {
                throw new CustomValidationException("Time slot is required.");
            }

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new CustomValidationException("Appointments cannot be booked with inactive doctors.");
            }

            var existingAppointments = await _appointmentRepository.GetAllAsync();

            var isSlotBooked = existingAppointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.ScheduledDate.Date == dto.ScheduledDate.Date &&
                a.TimeSlot == dto.TimeSlot &&
                a.Status != AppointmentStatus.Cancelled);

            if (isSlotBooked)
            {
                throw new CustomValidationException("Selected time slot is already booked.");
            }

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Pending;

            await _appointmentRepository.AddAsync(appointment);

            await RemoveDoctorAvailabilityCacheAsync(
                doctor.DoctorId,
                appointment.ScheduledDate,
                "Appointment booked");

            if (_publishEndpoint != null)
            {
                try
                {
                    await _publishEndpoint.Publish(new AppointmentBookedEvent
                    {
                        EventType = "AppointmentBooked",
                        AppointmentId = appointment.AppointmentId,
                        PatientId = patient.PatientId,
                        PatientName = patient.FullName,
                        DoctorId = doctor.DoctorId,
                        ScheduledDate = appointment.ScheduledDate,
                        TimeSlot = appointment.TimeSlot,
                        OccurredAt = DateTime.UtcNow
                    });

                    _logger?.LogInformation(
                        "AppointmentBookedEvent published using MassTransit for AppointmentId {AppointmentId}, DoctorId {DoctorId}",
                        appointment.AppointmentId,
                        doctor.DoctorId);
                }
                catch (Exception exception)
                {
                    _logger?.LogError(
                        exception,
                        "Appointment was booked, but failed to publish AppointmentBookedEvent for AppointmentId {AppointmentId}",
                        appointment.AppointmentId);
                }
            }

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new CustomValidationException("Cancelled appointments cannot be modified.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new CustomValidationException("Completed appointments cannot be modified.");
            }

            if (appointment.Status == dto.Status)
            {
                throw new CustomValidationException($"Appointment is already {dto.Status}.");
            }

            if (appointment.Status == AppointmentStatus.Pending &&
                dto.Status == AppointmentStatus.Completed)
            {
                throw new CustomValidationException("Pending appointments must be confirmed before completion.");
            }

            switch (dto.Status)
            {
                case AppointmentStatus.Confirmed:
                    appointment.Confirm();
                    break;

                case AppointmentStatus.Cancelled:
                    appointment.Cancel(dto.CancellationReason ?? string.Empty);
                    break;

                case AppointmentStatus.Completed:
                    appointment.Complete();
                    break;

                case AppointmentStatus.Pending:
                    throw new CustomValidationException("Cannot revert appointment to pending.");
            }

            await _appointmentRepository.UpdateAsync(id, appointment, CancellationToken.None);

            await RemoveDoctorAvailabilityCacheAsync(
                appointment.DoctorId,
                appointment.ScheduledDate,
                $"Appointment status changed to {appointment.Status}");

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new CustomValidationException("Completed appointments cannot be deleted.");
            }

            if (appointment.Status == AppointmentStatus.Confirmed)
            {
                throw new CustomValidationException("Confirmed appointments cannot be deleted.");
            }

            await _appointmentRepository.DeleteAsync(id);

            await RemoveDoctorAvailabilityCacheAsync(
                appointment.DoctorId,
                appointment.ScheduledDate,
                "Appointment deleted");

            return true;
        }

        private async Task RemoveDoctorAvailabilityCacheAsync(
            int doctorId,
            DateTime scheduledDate,
            string reason,
            CancellationToken ct = default)
        {
            if (_cache == null)
            {
                return;
            }

            var cacheKey = $"doctors:{doctorId}:availability:{scheduledDate:yyyy-MM-dd}";

            await _cache.RemoveAsync(cacheKey, ct);

            _logger?.LogInformation(
                "{CacheRemovedMessage}",
                BuildCacheRemovedMessage(doctorId, scheduledDate, cacheKey, reason));
        }

        private static string BuildCacheRemovedMessage(
            int doctorId,
            DateTime date,
            string cacheKey,
            string reason)
        {
            return
                Environment.NewLine +
                "========================================" + Environment.NewLine +
                "CACHE REMOVED - DOCTOR AVAILABILITY" + Environment.NewLine +
                "========================================" + Environment.NewLine +
                $"Doctor Id : {doctorId}" + Environment.NewLine +
                $"Date      : {date:yyyy-MM-dd}" + Environment.NewLine +
                $"Key       : {cacheKey}" + Environment.NewLine +
                $"Reason    : {reason}" + Environment.NewLine +
                "========================================";
        }
    }
}