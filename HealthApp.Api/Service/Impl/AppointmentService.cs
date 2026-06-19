using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HospitalManagementAPI.Model;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> Add(AppointmentDto dto)
        {
            if (dto == null)
                throw new AppointmentRuleException("Appointment details are required.");

            if (dto.DoctorId <= 0 || dto.PatientId <= 0)
                throw new AppointmentRuleException("Invalid doctor or patient reference.");

            if (dto.ScheduledDate.Date < DateTime.Today)
                throw new AppointmentRuleException("Appointment date cannot be in the past.");

            if (string.IsNullOrWhiteSpace(dto.TimeSlot))
                throw new AppointmentRuleException("Time slot is required.");

            var isBooked = await _repo.IsSlotBookedAsync(dto.DoctorId, dto.ScheduledDate.Date, dto.TimeSlot);

            if (isBooked)
                throw new ConflictException("This slot is already booked for the doctor.");

            var appointment = _mapper.Map<Appointment>(dto);
            var savedAppointment = await _repo.addAsync(appointment);

            if (savedAppointment == null)
                throw new AppointmentRuleException("Unable to create appointment.");

            return _mapper.Map<AppointmentDto>(savedAppointment);
        }

        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var appointments = await _repo.getallAsync();
            return _mapper.Map<List<AppointmentDto>>(appointments ?? new List<Appointment>());
        }

        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            if (id <= 0)
                throw new AppointmentRuleException("Invalid appointment id.");

            var appointment = await _repo.getbyidAsync(id);

            if (appointment == null)
                throw new EntityNotFoundException("Appointment", id);

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<AppointmentDto> CancelAppointment(int appointmentId, string reason)
        {
            if (appointmentId <= 0)
                throw new AppointmentRuleException("Invalid appointment id.");

            if (string.IsNullOrWhiteSpace(reason))
                throw new AppointmentRuleException("Cancellation reason is required.");

            var existing = await _repo.getbyidAsync(appointmentId);

            if (existing == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            if (existing.Status == "Completed")
                throw new ConflictException("Completed appointment cannot be cancelled.");

            if (existing.Status == "Cancelled")
                throw new ConflictException("Appointment is already cancelled.");

            var saved = await _repo.CancelAppointmentAsync(appointmentId, reason);

            if (saved == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            if (doctorId <= 0)
                throw new AppointmentRuleException("Invalid doctor id.");

            if (date.Date < DateTime.Today)
                throw new AppointmentRuleException("Cannot check availability for past date.");

            var bookedSlots = await _repo.GetBookedSlotsAsync(doctorId, date.Date);
            return bookedSlots;
        }

        public async Task<AppointmentDto> CompleteAppointment(int appointmentId)
        {
            if (appointmentId <= 0)
                throw new AppointmentRuleException("Invalid appointment id.");

            var existing = await _repo.getbyidAsync(appointmentId);

            if (existing == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            if (existing.Status == "Cancelled")
                throw new ConflictException("Cancelled appointment cannot be completed.");

            if (existing.Status == "Completed")
                throw new ConflictException("Appointment already completed.");

            if (existing.Status != "Confirmed")
                throw new AppointmentRuleException("Only confirmed appointments can be completed.");

            var saved = await _repo.UpdateStatusAsync(appointmentId, "Completed");

            if (saved == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto> ConfirmAppointment(int appointmentId)
        {
            if (appointmentId <= 0)
                throw new AppointmentRuleException("Invalid appointment id.");

            var existing = await _repo.getbyidAsync(appointmentId);

            if (existing == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            if (existing.Status == "Cancelled")
                throw new ConflictException("Cancelled appointment cannot be confirmed.");

            if (existing.Status == "Completed")
                throw new ConflictException("Completed appointment cannot be confirmed.");

            if (existing.Status == "Confirmed")
                throw new ConflictException("Appointment is already confirmed.");

            var saved = await _repo.UpdateStatusAsync(appointmentId, "Confirmed");

            if (saved == null)
                throw new EntityNotFoundException("Appointment", appointmentId);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(int doctorId, DateTime fromDate, DateTime toDate)
        {
            if (doctorId <= 0)
                throw new AppointmentRuleException("Invalid doctor id.");

            if (fromDate > toDate)
                throw new AppointmentRuleException("Invalid date range.");

            var appointments = await _repo.GetUpcomingByDoctorAsync(doctorId, fromDate, toDate);
            return _mapper.Map<List<AppointmentDto>>(appointments ?? new List<Appointment>());
        }

        public async Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot)
        {
            if (doctorId <= 0 || string.IsNullOrWhiteSpace(timeSlot))
                throw new AppointmentRuleException("Invalid slot check input.");

            return await _repo.IsSlotBookedAsync(doctorId, date, timeSlot);
        }


        public async Task<List<Appointment>> GetAppointmentsByPatientAndDoctor(int? patientId, int? doctorId)
        {
            if (patientId == null || doctorId == null)
                throw new AppointmentRuleException("Patient and Doctor id must be provided.");

            var appointment = await _repo.GetByPatientAndDoctor(patientId, doctorId);

            if (appointment == null)
                throw new EntityNotFoundException("Appointment", 0);

            return appointment;
        }
    }
}
