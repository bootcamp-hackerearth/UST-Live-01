using AutoMapper;
using HealthAxis.API.Events;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using MassTransit;
using CustomValidationException =
    HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment>
            _appointmentRepository;

        private readonly IRepository<Doctor>
            _doctorRepository;

        private readonly IRepository<Patient>
            _patientRepository;

        private readonly IMapper _mapper;

        private readonly IPublishEndpoint?
            _publishEndpoint;

        private readonly ILogger<AppointmentService>?
            _logger;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Patient> patientRepository,
            IMapper mapper,
            IPublishEndpoint? publishEndpoint = null,
            ILogger<AppointmentService>? logger = null)
        {
            _appointmentRepository =
                appointmentRepository;

            _doctorRepository =
                doctorRepository;

            _patientRepository =
                patientRepository;

            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAllAsync()
        {
            var appointments =
                await _appointmentRepository
                    .GetAllAsync();

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                appointments);
        }

        public async Task<AppointmentDto> AddAsync(
            CreateAppointmentDto dto)
        {
            if (
                dto.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new CustomValidationException(
                    "Appointments cannot be booked for past dates.");
            }

            if (
                dto.ScheduledDate.Date >
                DateTime.Today.AddMonths(6))
            {
                throw new CustomValidationException(
                    "Appointments can only be booked up to 6 months in advance.");
            }

            if (
                string.IsNullOrWhiteSpace(
                    dto.TimeSlot))
            {
                throw new CustomValidationException(
                    "Time slot is required.");
            }

            var patient =
                await _patientRepository
                    .GetByIdAsync(dto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            var doctor =
                await _doctorRepository
                    .GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException(
                    "Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new CustomValidationException(
                    "Appointments cannot be booked with inactive doctors.");
            }

            var existingAppointments =
                await _appointmentRepository
                    .GetAllAsync();

            var isSlotBooked =
                existingAppointments.Any(
                    appointment =>
                        appointment.DoctorId ==
                            dto.DoctorId &&
                        appointment
                            .ScheduledDate
                            .Date ==
                            dto.ScheduledDate.Date &&
                        appointment.TimeSlot ==
                            dto.TimeSlot &&
                        appointment.Status !=
                            AppointmentStatus.Cancelled);

            if (isSlotBooked)
            {
                throw new CustomValidationException(
                    "Selected time slot is already booked.");
            }

            var appointment =
                _mapper.Map<Appointment>(dto);

            appointment.Status =
                AppointmentStatus.Pending;

            await _appointmentRepository
                .AddAsync(appointment);

            await PublishAppointmentBookedEventAsync(
                appointment,
                patient,
                doctor);

            return _mapper.Map<AppointmentDto>(
                appointment);
        }

        public async Task<AppointmentDto>
            UpdateStatusAsync(
                int id,
                UpdateAppointmentStatusDto dto)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            if (
                appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new CustomValidationException(
                    "Cancelled appointments cannot be modified.");
            }

            if (
                appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new CustomValidationException(
                    "Completed appointments cannot be modified.");
            }

            if (
                appointment.Status ==
                dto.Status)
            {
                throw new CustomValidationException(
                    $"Appointment is already {dto.Status}.");
            }

            if (
                appointment.Status ==
                    AppointmentStatus.Pending &&
                dto.Status ==
                    AppointmentStatus.Completed)
            {
                throw new CustomValidationException(
                    "Pending appointments must be confirmed before completion.");
            }

            switch (dto.Status)
            {
                case AppointmentStatus.Confirmed:
                    appointment.Confirm();
                    break;

                case AppointmentStatus.Cancelled:
                    appointment.Cancel(
                        dto.CancellationReason ??
                        string.Empty);
                    break;

                case AppointmentStatus.Completed:
                    appointment.Complete();
                    break;

                case AppointmentStatus.Pending:
                    throw new CustomValidationException(
                        "Cannot revert appointment to pending.");

                default:
                    throw new CustomValidationException(
                        "Invalid appointment status.");
            }

            await _appointmentRepository
                .UpdateAsync(
                    id,
                    appointment,
                    CancellationToken.None);

            return _mapper.Map<AppointmentDto>(
                appointment);
        }

        public async Task<bool> DeleteAsync(
            int id)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            if (
                appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new CustomValidationException(
                    "Completed appointments cannot be deleted.");
            }

            if (
                appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                throw new CustomValidationException(
                    "Confirmed appointments cannot be deleted.");
            }

            await _appointmentRepository
                .DeleteAsync(id);

            return true;
        }

        private async Task
            PublishAppointmentBookedEventAsync(
                Appointment appointment,
                Patient patient,
                Doctor doctor)
        {
            if (_publishEndpoint == null)
            {
                _logger?.LogWarning(
                    "Appointment {AppointmentId} was booked, but the message publisher is unavailable.",
                    appointment.AppointmentId);

                return;
            }

            try
            {
                var appointmentEvent =
                    new AppointmentBookedEvent
                    {
                        EventType =
                            "AppointmentBooked",

                        AppointmentId =
                            appointment.AppointmentId,

                        PatientId =
                            patient.PatientId,

                        PatientName =
                            patient.FullName,

                        DoctorId =
                            doctor.DoctorId,

                        ScheduledDate =
                            appointment.ScheduledDate,

                        TimeSlot =
                            appointment.TimeSlot,

                        OccurredAt =
                            DateTime.UtcNow
                    };

                await _publishEndpoint.Publish(
                    appointmentEvent);

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
    }
}