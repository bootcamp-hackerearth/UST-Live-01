using AutoMapper;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IHealthRecordRepository healthRecordRepository,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _healthRecordRepository = healthRecordRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAllAppointmentsAsync()
        {
            var appointments =
                await _appointmentRepository
                    .GetAllAsync();

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                    appointments);
        }

        public async Task<AppointmentDto>
            GetAppointmentByIdAsync(
                int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            return _mapper.Map<
                AppointmentDto>(
                    appointment);
        }

        public async Task<int>
            AddAppointmentAsync(
                CreateAppointmentDto appointmentDto)
        {
            var patient =
                await _patientRepository
                    .GetByIdAsync(
                        appointmentDto.PatientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var doctor =
                await _doctorRepository
                    .GetByIdAsync(
                        appointmentDto.DoctorId);

            if (doctor == null)
            {
                throw new DoctorNotFoundException();
            }

            if (appointmentDto.ScheduledDate.Date <
                DateTime.Today)
            {
                throw new PastDateException();
            }

            if (appointmentDto.ScheduledDate.Date >
                DateTime.Today.AddMonths(6))
            {
                throw new AdvanceBookingLimitException();
            }

            if (!doctor.IsActive)
            {
                throw new DoctorUnavailableException();
            }

            bool available =
                await _appointmentRepository
                    .IsSlotAvailableAsync(
                        appointmentDto.DoctorId,
                        appointmentDto.ScheduledDate,
                        appointmentDto.TimeSlot);

            if (!available)
            {
                throw new AppointmentConflictException();
            }

            var appointment =
                _mapper.Map<Appointment>(
                    appointmentDto);

            appointment.Status =
                AppointmentStatus.Pending;

            await _appointmentRepository
                .AddAsync(
                    appointment);

            await _context
                .SaveChangesAsync();

            return appointment.AppointmentId;
        }

        public async Task
            UpdateAppointmentAsync(
                int appointmentId,
                UpdateAppointmentDto appointmentDto)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            bool available =
                await _appointmentRepository
                    .IsSlotAvailableAsync(
                        appointmentDto.DoctorId,
                        appointmentDto.ScheduledDate,
                        appointmentDto.TimeSlot);

            if (!available)
            {
                throw new AppointmentConflictException();
            }

            if (appointment.Status ==
                AppointmentStatus.Completed)
            {
                throw new InvalidAppointmentStatusException(
                    "Completed appointments cannot be modified.");
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new InvalidAppointmentStatusException(
                    "Cancelled appointments cannot be modified.");
            }

            appointment.DoctorId =
                appointmentDto.DoctorId;

            appointment.ScheduledDate =
                appointmentDto.ScheduledDate;

            appointment.TimeSlot =
                appointmentDto.TimeSlot;

            await _appointmentRepository
                .UpdateAsync(
                    appointment);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            DeleteAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status ==
                AppointmentStatus.Pending ||
                appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                throw new AppointmentDeletionException();
            }

            await _appointmentRepository
                .DeleteAsync(
                    appointmentId);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            ConfirmAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Pending)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants.ConfirmOnlyPending);
            }

            appointment.Confirm();

            await _appointmentRepository
                .UpdateAsync(
                    appointment);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            CancelAppointmentAsync(
                int appointmentId,
                string reason)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Pending &&
                appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                            .CancelOnlyPendingOrConfirmed);
            }

            appointment.Cancel(reason);

            await _appointmentRepository
                .UpdateAsync(
                    appointment);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            CompleteAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _appointmentRepository
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new
                    InvalidAppointmentStatusException(
                        Constants
                            .CompleteOnlyConfirmed);
            }

            appointment.Complete();

            await _appointmentRepository
                .UpdateAsync(
                    appointment);

            await _context
                .SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByPatientAsync(
                int patientId)
        {
            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByPatientAsync(
                        patientId);

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                    appointments);
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByDoctorAsync(
                int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByDoctorAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _healthRecordRepository
                    .GetRecordedAppointmentIdsAsync();

            var appointmentDtos =
                _mapper.Map<List<AppointmentDto>>(
                    appointments);

            foreach (var appointment in appointmentDtos)
            {
                appointment.HasHealthRecord =
                    recordedAppointmentIds.Contains(
                        appointment.AppointmentId);
            }

            return appointmentDtos;
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetTodayScheduleAsync(
                int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetTodayScheduleAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _healthRecordRepository
                    .GetRecordedAppointmentIdsAsync();

            var appointmentDtos =
                _mapper.Map<List<AppointmentDto>>(
                    appointments);

            foreach (var appointment in appointmentDtos)
            {
                appointment.HasHealthRecord =
                    recordedAppointmentIds.Contains(
                        appointment.AppointmentId);
            }

            return appointmentDtos;
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetWeeklyScheduleAsync(
                int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetWeeklyScheduleAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _healthRecordRepository
                    .GetRecordedAppointmentIdsAsync();

            var appointmentDtos =
                _mapper.Map<List<AppointmentDto>>(
                    appointments);

            foreach (var appointment in appointmentDtos)
            {
                appointment.HasHealthRecord =
                    recordedAppointmentIds.Contains(
                        appointment.AppointmentId);
            }

            return appointmentDtos;
        }

        public async Task<AppointmentDto>
            GetNextAppointmentByPatientAsync(
                int patientId)
        {
            var appointment =
                await _appointmentRepository
                    .GetNextAppointmentByPatientAsync(
                        patientId);

            if (appointment == null)
            {
                return null;
            }

            return _mapper.Map<
                AppointmentDto>(
                    appointment);
        }
    }
}