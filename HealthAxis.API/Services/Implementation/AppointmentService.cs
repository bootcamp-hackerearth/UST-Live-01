using AutoMapper;
using HealthAxis.API.DTO.AppointmentDtos;
using HealthAxis.API.Enums;
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
                throw new NotFoundException("Appointment not found");
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

        public async Task<AppointmentDto> AddAsync(
            CreateAppointmentDto appointmentDto)
        {
            var patient = await patientRepository.GetByIdAsync(
                appointmentDto.PatientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var doctor = await doctorRepository.GetByIdAsync(
                appointmentDto.DoctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found");
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException(
                    "Doctor is inactive. Appointment cannot be booked");
            }

            if (appointmentDto.ScheduledDate == default)
            {
                throw new ValidationException("Appointment date is required");
            }

            if (appointmentDto.ScheduledDate.Date < DateTime.Today)
            {
                throw new ValidationException(
                    "Appointment date cannot be in the past");
            }

            if (appointmentDto.ScheduledDate.Date > DateTime.Today.AddMonths(6))
            {
                throw new BusinessRuleException(
                    "Appointments can only be booked up to 6 months in advance");
            }

            if (string.IsNullOrWhiteSpace(appointmentDto.TimeSlot))
            {
                throw new ValidationException("Time slot is required");
            }

            var appointments = await appointmentRepository.GetAllAsync();

            bool doctorAlreadyBooked = appointments.Any(a =>
                a.DoctorId == appointmentDto.DoctorId &&
                a.ScheduledDate.Date == appointmentDto.ScheduledDate.Date &&
                a.TimeSlot == appointmentDto.TimeSlot &&
                a.Status != AppointmentStatus.Cancelled);

            if (doctorAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "Doctor is already booked for this date and time slot");
            }

            bool patientAlreadyBooked = appointments.Any(a =>
                a.PatientId == appointmentDto.PatientId &&
                a.ScheduledDate.Date == appointmentDto.ScheduledDate.Date &&
                a.TimeSlot == appointmentDto.TimeSlot &&
                a.Status != AppointmentStatus.Cancelled);

            if (patientAlreadyBooked)
            {
                throw new BusinessRuleException(
                    "Patient already has an appointment for this date and time slot");
            }

            var appointment = new Appointment
            {
                PatientId = appointmentDto.PatientId,
                DoctorId = appointmentDto.DoctorId,
                ScheduledDate = appointmentDto.ScheduledDate,
                TimeSlot = appointmentDto.TimeSlot,
                Status = AppointmentStatus.Pending,
                CancellationReason = null
            };

            var saved = await appointmentRepository.AddAsync(appointment);

            return mapper.Map<AppointmentDto>(saved);
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

            if (appointment.Status == AppointmentStatus.Completed &&
                statusDto.Status != AppointmentStatus.Completed)
            {
                throw new BusinessRuleException(
                    "Completed appointment status cannot be changed");
            }

            if (appointment.Status == AppointmentStatus.Cancelled &&
                statusDto.Status != AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Cancelled appointment cannot be completed or confirmed");
            }

            if (statusDto.Status == AppointmentStatus.Cancelled &&
                appointment.Status != AppointmentStatus.Pending)
            {
                throw new BusinessRuleException(
                    "Only pending appointments can be cancelled");
            }

            if (appointment.Status == AppointmentStatus.Pending &&
                statusDto.Status == AppointmentStatus.Completed)
            {
                throw new BusinessRuleException(
                    "Pending appointment cannot directly be completed. First confirm it, then complete it");
            }

            if (appointment.Status == AppointmentStatus.Confirmed &&
                statusDto.Status == AppointmentStatus.Cancelled)
            {
                throw new BusinessRuleException(
                    "Confirmed appointment cannot be cancelled");
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

        public async Task<AppointmentDto> DeleteAsync(int id)
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