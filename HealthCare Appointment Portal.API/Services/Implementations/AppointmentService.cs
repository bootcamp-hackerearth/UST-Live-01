using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.AppointmentDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class AppointmentService
        : IAppointmentService
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
        {
            _appointmentRepository =
                appointmentRepository;

            _doctorRepository =
                doctorRepository;

            _patientRepository =
                patientRepository;

            _mapper =
                mapper;
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
                    .IsSlotAvailableForUpdateAsync(
                        appointmentId,
                        appointmentDto.DoctorId,
                        appointmentDto.ScheduledDate,
                        appointmentDto.TimeSlot);

            if (!available)
            {
                throw new AppointmentConflictException();
            }

            if (appointment.Status ==
                    AppointmentStatus.Completed ||
                appointment.Status ==
                    AppointmentStatus.Cancelled)
            {
                throw new InvalidAppointmentStatusException(
                    "This appointment cannot be modified.");
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
                throw new InvalidAppointmentStatusException(
                    Constants.ConfirmOnlyPending);
            }

            appointment.Confirm();

            await _appointmentRepository
                .UpdateAsync(
                    appointment);
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
                throw new InvalidAppointmentStatusException(
                    Constants.CancelOnlyPendingOrConfirmed);
            }

            appointment.Cancel(
                reason);

            await _appointmentRepository
                .UpdateAsync(
                    appointment);
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
                throw new InvalidAppointmentStatusException(
                    Constants.CompleteOnlyConfirmed);
            }

            appointment.Complete();

            await _appointmentRepository
                .UpdateAsync(
                    appointment);
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

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                    appointments);
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetTodayScheduleAsync(
                int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetTodayScheduleAsync(
                        doctorId);

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                    appointments);
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetWeeklyScheduleAsync(
                int doctorId)
        {
            var appointments =
                await _appointmentRepository
                    .GetWeeklyScheduleAsync(
                        doctorId);

            return _mapper.Map<
                IEnumerable<AppointmentDto>>(
                    appointments);
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