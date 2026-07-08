using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Messages;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;
using MassTransit;

namespace HealthAxis.API.Services
{
    public class AppointmentService
        : Service<Appointment, AppointmentReadDto, AppointmentCreateDto, AppointmentUpdateDto>,
          IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            ILogger<AppointmentService> logger)
            : base(appointmentRepository, mapper)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public new async Task<AppointmentReadDto> CreateAsync(
            AppointmentCreateDto createDto,
            CancellationToken ct = default)
        {
            if (createDto.ScheduledDate.Date < DateTime.Today)
            {
                throw new InvalidOperationException(
                    "Past dates are not allowed.");
            }

            if (string.IsNullOrWhiteSpace(createDto.TimeSlot))
            {
                throw new InvalidOperationException(
                    "Time slot is required.");
            }

            if (createDto.ScheduledDate.Date == DateTime.Today &&
                IsPastSlot(createDto.TimeSlot))
            {
                throw new InvalidOperationException(
                    "Past time slots are not allowed.");
            }

            DateTime maxAllowedDate =
                DateTime.Today.AddMonths(6);

            if (createDto.ScheduledDate.Date > maxAllowedDate)
            {
                throw new InvalidOperationException(
                    "Appointments can only be booked up to 6 months in advance.");
            }

            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(
                    createDto.DoctorId,
                    ct);

            if (doctor == null)
            {
                throw new InvalidOperationException(
                    "Doctor not found.");
            }

            if (!doctor.IsActive)
            {
                throw new InvalidOperationException(
                    "This doctor is currently inactive and cannot accept appointments.");
            }

            Patient? patient =
                await _patientRepository.GetByIdAsync(
                    createDto.PatientId,
                    ct);

            if (patient == null)
            {
                throw new InvalidOperationException(
                    "Patient not found.");
            }

            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            bool doctorAlreadyBooked =
                appointments.Any(appointment =>
                    appointment.DoctorId == createDto.DoctorId &&
                    appointment.ScheduledDate.Date == createDto.ScheduledDate.Date &&
                    appointment.TimeSlot == createDto.TimeSlot &&
                    appointment.Status != AppointmentStatus.Cancelled);

            if (doctorAlreadyBooked)
            {
                throw new InvalidOperationException(
                    "This doctor is already booked for the selected date and time slot.");
            }

            bool patientAlreadyBookedAtSameTime =
                appointments.Any(appointment =>
                    appointment.PatientId == createDto.PatientId &&
                    appointment.ScheduledDate.Date == createDto.ScheduledDate.Date &&
                    appointment.TimeSlot == createDto.TimeSlot &&
                    appointment.Status != AppointmentStatus.Cancelled);

            if (patientAlreadyBookedAtSameTime)
            {
                throw new InvalidOperationException(
                    "You already have an appointment booked at this date and time slot.");
            }

            Appointment appointmentToCreate =
                _mapper.Map<Appointment>(createDto);

            appointmentToCreate.ScheduledDate =
                createDto.ScheduledDate.Date;

            appointmentToCreate.Status =
                AppointmentStatus.Scheduled;

            Appointment createdAppointment =
                await _appointmentRepository.CreateAsync(
                    appointmentToCreate,
                    ct);

            AppointmentBookedEvent appointmentBookedEvent = new()
            {
                AppointmentId = createdAppointment.AppointmentId,
                PatientName = patient.FullName,
                DoctorId = createdAppointment.DoctorId,
                ScheduledDate = createdAppointment.ScheduledDate,
                TimeSlot = createdAppointment.TimeSlot
            };

            _logger.LogInformation(
                "Publishing AppointmentBookedEvent for AppointmentId {AppointmentId}, DoctorId {DoctorId}",
                appointmentBookedEvent.AppointmentId,
                appointmentBookedEvent.DoctorId);

            await _publishEndpoint.Publish(
                appointmentBookedEvent,
                ct);

            _logger.LogInformation(
                "AppointmentBookedEvent published successfully for AppointmentId {AppointmentId}",
                appointmentBookedEvent.AppointmentId);

            return await MapAppointmentWithNamesAsync(
                createdAppointment,
                ct);
        }

        public async Task<List<AppointmentReadDto>> GetAllWithDetailsAsync(
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            return await MapAppointmentsWithNamesAsync(
                appointments,
                ct);
        }

        public async Task<List<AppointmentReadDto>> GetAppointmentsByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<Appointment> filteredAppointments =
                appointments
                    .Where(appointment =>
                        appointment.PatientId == patientId)
                    .OrderByDescending(appointment =>
                        appointment.ScheduledDate)
                    .ToList();

            return await MapAppointmentsWithNamesAsync(
                filteredAppointments,
                ct);
        }

        public async Task<List<AppointmentReadDto>> GetAppointmentsByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<Appointment> filteredAppointments =
                appointments
                    .Where(appointment =>
                        appointment.DoctorId == doctorId)
                    .OrderByDescending(appointment =>
                        appointment.ScheduledDate)
                    .ToList();

            return await MapAppointmentsWithNamesAsync(
                filteredAppointments,
                ct);
        }

        public async Task<AppointmentReadDto?> UpdateStatusAsync(
            int appointmentId,
            AppointmentStatusUpdateDto statusUpdateDto,
            CancellationToken ct = default)
        {
            Appointment? appointment =
                await _appointmentRepository.GetByIdAsync(
                    appointmentId,
                    ct);

            if (appointment == null)
            {
                return null;
            }

            if (statusUpdateDto.Status == AppointmentStatus.Confirmed)
            {
                appointment.Confirm();
            }
            else if (statusUpdateDto.Status == AppointmentStatus.Cancelled)
            {
                appointment.Cancel(
                    statusUpdateDto.CancellationReason ?? string.Empty);
            }
            else if (statusUpdateDto.Status == AppointmentStatus.Completed)
            {
                appointment.Complete();
            }
            else
            {
                appointment.Status =
                    statusUpdateDto.Status;
            }

            await _appointmentRepository.SaveChangesAsync(ct);

            return await MapAppointmentWithNamesAsync(
                appointment,
                ct);
        }

        public async Task<List<AppointmentReportDto>> GetAppointmentReportAsync(
            CancellationToken ct = default)
        {
            List<Appointment> appointments =
                await _appointmentRepository.GetAllAsync(ct);

            List<AppointmentReportDto> report =
                appointments
                    .GroupBy(appointment =>
                        appointment.ScheduledDate.Date)
                    .Select(group =>
                        new AppointmentReportDto
                        {
                            Date = group.Key,

                            TotalCount = group.Count(),

                            ScheduledCount = group.Count(appointment =>
                                appointment.Status == AppointmentStatus.Scheduled),

                            ConfirmedCount = group.Count(appointment =>
                                appointment.Status == AppointmentStatus.Confirmed),

                            CancelledCount = group.Count(appointment =>
                                appointment.Status == AppointmentStatus.Cancelled),

                            CompletedCount = group.Count(appointment =>
                                appointment.Status == AppointmentStatus.Completed)
                        })
                    .OrderBy(reportItem =>
                        reportItem.Date)
                    .ToList();

            return report;
        }

        private async Task<List<AppointmentReadDto>> MapAppointmentsWithNamesAsync(
            List<Appointment> appointments,
            CancellationToken ct)
        {
            List<Doctor> doctors =
                await _doctorRepository.GetAllAsync(ct);

            List<Patient> patients =
                await _patientRepository.GetAllAsync(ct);

            List<AppointmentReadDto> result =
                appointments
                    .OrderByDescending(appointment =>
                        appointment.ScheduledDate)
                    .Select(appointment =>
                    {
                        Doctor? doctor =
                            doctors.FirstOrDefault(doctor =>
                                doctor.DoctorId == appointment.DoctorId);

                        Patient? patient =
                            patients.FirstOrDefault(patient =>
                                patient.PatientId == appointment.PatientId);

                        return new AppointmentReadDto
                        {
                            AppointmentId = appointment.AppointmentId,

                            PatientId = appointment.PatientId,
                            PatientName = patient?.FullName ?? "Unknown Patient",

                            DoctorId = appointment.DoctorId,
                            DoctorName = doctor?.FullName ?? "Unknown Doctor",

                            ScheduledDate = appointment.ScheduledDate,
                            TimeSlot = appointment.TimeSlot,

                            Status = appointment.Status,
                            CancellationReason = appointment.CancellationReason
                        };
                    })
                    .ToList();

            return result;
        }

        private async Task<AppointmentReadDto> MapAppointmentWithNamesAsync(
            Appointment appointment,
            CancellationToken ct)
        {
            Doctor? doctor =
                await _doctorRepository.GetByIdAsync(
                    appointment.DoctorId,
                    ct);

            Patient? patient =
                await _patientRepository.GetByIdAsync(
                    appointment.PatientId,
                    ct);

            return new AppointmentReadDto
            {
                AppointmentId = appointment.AppointmentId,

                PatientId = appointment.PatientId,
                PatientName = patient?.FullName ?? "Unknown Patient",

                DoctorId = appointment.DoctorId,
                DoctorName = doctor?.FullName ?? "Unknown Doctor",

                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,

                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason
            };
        }

        private static bool IsPastSlot(
            string timeSlot)
        {
            string startTime =
                timeSlot.Split('-')[0];

            if (!TimeSpan.TryParse(startTime, out TimeSpan slotStartTime))
            {
                return true;
            }

            TimeSpan currentTime =
                DateTime.Now.TimeOfDay;

            return slotStartTime <= currentTime;
        }
    }
}