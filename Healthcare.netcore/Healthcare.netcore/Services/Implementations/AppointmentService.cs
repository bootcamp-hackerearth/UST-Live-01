using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Enums;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using CustomValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IRepository<Appointment> appointmentRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Patient> patientRepository,
            IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        // Add Appointment
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

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = AppointmentStatus.Pending;

            await _appointmentRepository.AddAsync(appointment);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        // Update Appointment Status
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
                    throw new CustomValidationException("Appointments cannot be reverted to pending status.");
            }

            await _appointmentRepository.UpdateAsync(id, appointment, CancellationToken.None);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        // Delete Appointment
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

            return true;
        }
    }
}