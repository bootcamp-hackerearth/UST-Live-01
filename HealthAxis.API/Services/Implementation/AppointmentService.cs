using AutoMapper;
using HealthAxis.Shared.DTO.AppointmentDtos;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Implementation
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper) : IAppointmentService
    {
        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(
                await appointmentRepository.GetAllAsync());
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                return null;
            }

            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<List<AppointmentDto>> GetByPatientIdAsync(int patientId)
        {
            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var appointments = await appointmentRepository.GetAllAsync();

            var patientAppointments = appointments
                .Where(a => a.PatientId == patientId)
                .ToList();

            return mapper.Map<List<AppointmentDto>>(patientAppointments);
        }

        public async Task<AppointmentDto> AddAsync(CreateAppointmentDto appointmentDto)
        {
            var patient = await patientRepository.GetByIdAsync(appointmentDto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var doctor = await doctorRepository.GetByIdAsync(appointmentDto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException(
                    "Doctor is not available for appointment");
            }

            if (appointmentDto.ScheduledDate.Date < DateTime.Today)
            {
                throw new ValidationException(
                    "Appointment date cannot be in the past");
            }

            if (appointmentDto.ScheduledDate.Date > DateTime.Today.AddMonths(6))
            {
                throw new ValidationException(
                    "Appointment date cannot be more than 6 months ahead");
            }

            if (string.IsNullOrWhiteSpace(appointmentDto.TimeSlot))
            {
                throw new ValidationException("Time slot is required");
            }

            var appointments = await appointmentRepository.GetAllAsync();

            var activeStatuses = new[]
            {
        AppointmentStatus.Pending,
        AppointmentStatus.Confirmed
    };

            var normalizedTimeSlot = appointmentDto.TimeSlot.Trim().ToLower();

            var doctorAlreadyBooked = appointments.Any(a =>
                a.DoctorId == appointmentDto.DoctorId &&
                a.ScheduledDate.Date == appointmentDto.ScheduledDate.Date &&
                a.TimeSlot.Trim().ToLower() == normalizedTimeSlot &&
                activeStatuses.Contains(a.Status));

            if (doctorAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "This doctor already has an appointment at this time");
            }

            var patientAlreadyBooked = appointments.Any(a =>
                a.PatientId == appointmentDto.PatientId &&
                a.ScheduledDate.Date == appointmentDto.ScheduledDate.Date &&
                a.TimeSlot.Trim().ToLower() == normalizedTimeSlot &&
                activeStatuses.Contains(a.Status));

            if (patientAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "You already have an appointment at this time");
            }

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                ScheduledDate = appointmentDto.ScheduledDate.Date,
                TimeSlot = appointmentDto.TimeSlot.Trim(),
                Status = AppointmentStatus.Pending
            };

            var savedAppointment = await appointmentRepository.AddAsync(appointment);

            return mapper.Map<AppointmentDto>(savedAppointment);
        }

        public async Task<AppointmentDto> UpdateStatusAsync(
     int id,
     UpdateAppointmentStatusDto statusDto)
        {
            var appointment = await appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            if (!Enum.IsDefined(typeof(AppointmentStatus), statusDto.Status))
            {
                throw new ValidationException("Invalid appointment status");
            }

            if (appointment.Status == AppointmentStatus.Completed ||
                appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Completed or cancelled appointment cannot be changed");
            }

            if (appointment.Status == AppointmentStatus.Pending &&
                statusDto.Status != AppointmentStatus.Confirmed &&
                statusDto.Status != AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Pending appointment can only be confirmed or cancelled");
            }

            if (appointment.Status == AppointmentStatus.Confirmed &&
                statusDto.Status != AppointmentStatus.Completed)
            {
                throw new BusinessRuleException(
                    "Confirmed appointment can only be completed");
            }

            if (statusDto.Status == AppointmentStatus.Cancelled &&
                string.IsNullOrWhiteSpace(statusDto.CancellationReason))
            {
                throw new ValidationException("Cancellation reason is required");
            }

            appointment.Status = statusDto.Status;

            if (statusDto.Status == AppointmentStatus.Cancelled)
            {
                appointment.CancellationReason = statusDto.CancellationReason;
            }
            else
            {
                appointment.CancellationReason = null;
            }

            var updated = await appointmentRepository.UpdateAsync(
                id,
                appointment);

            return mapper.Map<AppointmentDto>(updated);
        }

        public async Task<AppointmentDto?> DeleteAsync(int id)
        {
            var deleted = await appointmentRepository.DeleteAsync(id);

            if (deleted == null)
            {
                throw new NotFoundException("Appointment not found");
            }

            return mapper.Map<AppointmentDto>(deleted);
        }
    }
}