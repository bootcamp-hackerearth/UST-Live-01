using AutoMapper;
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
    public class AppointmentService
        : IAppointmentService
    {
        private readonly IUnitOfWork
            _unitOfWork;

        private readonly IMapper
            _mapper;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork =
                unitOfWork;

            _mapper =
                mapper;
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAllAppointmentsAsync()
        {
            var appointments =
                await _unitOfWork
                    .Appointments
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
                await _unitOfWork
                    .Appointments
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
                await _unitOfWork
                    .Patients
                    .GetByIdAsync(
                        appointmentDto.PatientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var doctor =
                await _unitOfWork
                    .Doctors
                    .GetByIdAsync(
                        appointmentDto.DoctorId);

            if (doctor == null)
            {
                throw new DoctorNotFoundException();
            }

            if (appointmentDto
                .ScheduledDate.Date
                < DateTime.Today)
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
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .AddAsync(
                    appointment);

            await _unitOfWork
                .CommitAsync();

            return appointment
                .AppointmentId;
        }

        public async Task
            UpdateAppointmentAsync(
                int appointmentId,
                UpdateAppointmentDto appointmentDto)
        {
            var appointment =
                await _unitOfWork
                    .Appointments
                    .GetByIdAsync(
                        appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            bool available =
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .UpdateAsync(
                    appointment);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            DeleteAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .DeleteAsync(
                    appointmentId);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            ConfirmAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .UpdateAsync(
                    appointment);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            CancelAppointmentAsync(
                int appointmentId,
                string reason)
        {
            var appointment =
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .UpdateAsync(
                    appointment);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            CompleteAppointmentAsync(
                int appointmentId)
        {
            var appointment =
                await _unitOfWork
                    .Appointments
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

            await _unitOfWork
                .Appointments
                .UpdateAsync(
                    appointment);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task<IEnumerable<AppointmentDto>>
            GetAppointmentsByPatientAsync(
                int patientId)
        {
            var appointments =
                await _unitOfWork
                    .Appointments
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
                await _unitOfWork
                    .Appointments
                    .GetAppointmentsByDoctorAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .Appointments
                    .GetTodayScheduleAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .Appointments
                    .GetWeeklyScheduleAsync(
                        doctorId);

            var recordedAppointmentIds =
                await _unitOfWork
                    .HealthRecords
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
                await _unitOfWork
                    .Appointments
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