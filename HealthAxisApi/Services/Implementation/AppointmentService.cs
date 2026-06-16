using AutoMapper;
using HealthAxisCore_Api.DTOs.Appointment;
using HealthAxisCore_Api.Enums;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;

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

        //  Get All
        public async Task<IEnumerable<AppointmentResponseDTO>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        //  Get By Id
        public async Task<AppointmentResponseDTO?> GetByIdAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);
            if (appt == null) return null;

            return _mapper.Map<AppointmentResponseDTO>(appt);
        }

        //  CREATE 
        public async Task<AppointmentResponseDTO> CreateAsync(CreateAppointmentDTO dto)
        {
            //  Check doctor availability (prevent double booking)
            bool isAvailable = await _doctorRepository.IsDoctorAvailable(
                dto.DoctorId,
                dto.ScheduledDate
            );

            if (!isAvailable)
                throw new Exception("Doctor not available for the selected date");

            var appointment = _mapper.Map<Appointment>(dto);

            appointment.Status = AppointmentStatus.Pending;
            appointment.CreatedDate = DateTime.Now;

            await _repository.AddAsync(appointment);

            return _mapper.Map<AppointmentResponseDTO>(appointment);
        }

        //  Delete
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        //  Get by Doctor
        public async Task<IEnumerable<AppointmentResponseDTO>> GetByDoctorAsync(int doctorId)
        {
            var data = await _repository.GetByDoctor(doctorId);
            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        //  Get by Patient
        public async Task<IEnumerable<AppointmentResponseDTO>> GetByPatientAsync(int patientId)
        {
            var data = await _repository.GetByPatient(patientId);
            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        //  FILTERING 
        public async Task<IEnumerable<AppointmentResponseDTO>> FilterAsync(
            AppointmentStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            var data = await _repository.FilterAppointments(status, startDate, endDate);
            return _mapper.Map<IEnumerable<AppointmentResponseDTO>>(data);
        }

        // ✅ CANCEL APPOINTMENT
        public async Task<bool> CancelAsync(int id, string reason)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
                return false;

            if (appt.Status == AppointmentStatus.Cancelled)
                throw new Exception("Appointment already cancelled");

            await _repository.CancelAppointment(id, reason);

            return true;
        }

        // ✅ CONFIRM APPOINTMENT
        public async Task<bool> ConfirmAsync(int id)
        {
            var appt = await _repository.GetByIdAsync(id);

            if (appt == null)
                return false;

            if (appt.Status == AppointmentStatus.Completed)
                throw new Exception("Cannot confirm completed appointment");

            await _repository.ConfirmAppointment(id);

            return true;
        }
    }
}
