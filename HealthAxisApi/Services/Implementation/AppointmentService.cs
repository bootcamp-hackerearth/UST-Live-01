using AutoMapper;
using HealthAxisCore_Api.DTOs.Appointment;
using HealthAxisCore_Api.Enums;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _repository = repository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        // ✅ Get All
        public async Task<IEnumerable<AppointmentResponseDTO>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        // ✅ Get By Id
        public async Task<AppointmentResponseDTO?> GetByIdAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
                throw new EntityNotFoundException("Appointment not found");

            return _mapper.Map<AppointmentResponseDTO>(appt);
        }

        // ✅ CREATE
        public async Task<AppointmentResponseDTO> CreateAsync(CreateAppointmentDTO dto)
        {
            // ✅ Business rule: Doctor availability
            bool isAvailable = await _doctorRepository.IsDoctorAvailable(
                dto.DoctorId,
                dto.ScheduledDate
            );

            if (!isAvailable)
                throw new AppointmentRuleException("Doctor not available for the selected date");

            var appointment = _mapper.Map<Appointment>(dto);

            appointment.Status = AppointmentStatus.Pending;
            appointment.CreatedDate = DateTime.Now;

            await _repository.AddAsync(appointment);

            return _mapper.Map<AppointmentResponseDTO>(appointment);
        }

        // ✅ Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                throw new EntityNotFoundException("Appointment not found");

            await _repository.DeleteAsync(id);

            return true;
        }

        // ✅ Get by Doctor
        public async Task<IEnumerable<AppointmentResponseDTO>> GetByDoctorAsync(int doctorId)
        {
            var data = await _repository.GetByDoctor(doctorId);

            if (data == null || !data.Any())
                throw new EntityNotFoundException("No appointments found for this doctor");

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        // ✅ Get by Patient
        public async Task<IEnumerable<AppointmentResponseDTO>> GetByPatientAsync(int patientId)
        {
            var data = await _repository.GetByPatient(patientId);

            if (data == null || !data.Any())
                throw new EntityNotFoundException("No appointments found for this patient");

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        // ✅ Filter
        public async Task<IEnumerable<AppointmentResponseDTO>> FilterAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var data = await _repository.FilterAppointments(status, startDate, endDate);

            if (data == null || !data.Any())
                throw new EntityNotFoundException("No appointments found for given criteria");

            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        // ✅ CANCEL APPOINTMENT
        public async Task<bool> CancelAsync(int id, string reason)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
                throw new EntityNotFoundException("Appointment not found");

            if (appt.Status == AppointmentStatus.Cancelled)
                throw new AppointmentRuleException("Appointment already cancelled");

            if (appt.Status == AppointmentStatus.Completed)
                throw new AppointmentRuleException("Cannot cancel a completed appointment");

            await _repository.CancelAppointment(id, reason);

            return true;
        }

        // ✅ CONFIRM APPOINTMENT
        public async Task<bool> ConfirmAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
                throw new EntityNotFoundException("Appointment not found");

            if (appt.Status == AppointmentStatus.Completed)
                throw new AppointmentRuleException("Cannot confirm a completed appointment");

            if (appt.Status == AppointmentStatus.Cancelled)
                throw new AppointmentRuleException("Cannot confirm a cancelled appointment");

            await _repository.ConfirmAppointment(id);

            return true;
        }
    }
}