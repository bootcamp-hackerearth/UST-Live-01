using AutoMapper;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

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

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
            : base(appointmentRepository, mapper)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
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
                    statusUpdateDto.CancellationReason);
            }
            else if (statusUpdateDto.Status == AppointmentStatus.Completed)
            {
                appointment.Complete();
            }
            else
            {
                appointment.Status = statusUpdateDto.Status;
            }

            await _appointmentRepository.SaveChangesAsync(ct);

            AppointmentReadDto dto =
                await MapAppointmentWithNamesAsync(
                    appointment,
                    ct);

            return dto;
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
    }
}
