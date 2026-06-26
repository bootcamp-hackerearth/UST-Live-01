using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo,IPatientRepository patientRepository,
                IDoctorRepository doctorRepository  , IMapper mapper)
        {
            _repo = repo;
            _patientRepository= patientRepository;
            _doctorRepository= doctorRepository;
            _mapper = mapper;
        }

        // ✅ CREATE APPOINTMENT (FIXED ✅)
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

            // ✅ Save basic appointment
            var appointment = _mapper.Map<Appointment>(dto);
            var savedAppointment = await _repo.addAsync(appointment);

            // ✅ IMPORTANT: reload with Patient + Doctor
            var fullAppointment = await _repo.getbyidAsync(savedAppointment.AppointmentId);

            // ✅ Now mapping works properly
            return _mapper.Map<AppointmentDto>(fullAppointment);
        }

        // ✅ GET ALL
        public async Task<List<AppointmentDto>> GetAllAppointments()
        {
            var appointments = await _repo.getallAsync();

            foreach (var appointment in appointments)
            {
                await LoadNavigation(appointment); 
    }

            return _mapper.Map<List<AppointmentDto>>(appointments);
        }

        // ✅ GET BY ID
        public async Task<AppointmentDto> GetAppointmentById(int id)
        {

            var appointment = await _repo.getbyidAsync(id);

            if (appointment == null)
                throw new EntityNotFoundException("Appointment", id);

            await LoadNavigation(appointment);
            return _mapper.Map<AppointmentDto>(appointment);

        }

        // ✅ CANCEL
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

            // ✅ Already includes Patient + Doctor (repo fixed)
            return _mapper.Map<AppointmentDto>(saved);
        }

        // ✅ CONFIRM
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

        // ✅ COMPLETE
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

        // ✅ CHECK AVAILABILITY
        public async Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            if (doctorId <= 0)
                throw new AppointmentRuleException("Invalid doctor id.");

            if (date.Date < DateTime.Today)
                throw new AppointmentRuleException("Cannot check availability for past date.");

            var bookedSlots = await _repo.GetBookedSlotsAsync(doctorId, date.Date);

            return bookedSlots ?? new List<string>();
        }

        // ✅ SLOT CHECK
        public async Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot)
        {
            if (doctorId <= 0 || string.IsNullOrWhiteSpace(timeSlot))
                throw new AppointmentRuleException("Invalid slot check input.");

            return await _repo.IsSlotBookedAsync(doctorId, date, timeSlot);
        }

        // ✅ UPCOMING
        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId, DateTime fromDate, DateTime toDate)
        {
            if (doctorId <= 0)
                throw new AppointmentRuleException("Invalid doctor id.");

            if (fromDate > toDate)
                throw new AppointmentRuleException("Invalid date range.");

            var appointments = await _repo.GetUpcomingByDoctorAsync(doctorId, fromDate, toDate);

            return _mapper.Map<List<AppointmentDto>>(
                appointments ?? new List<Appointment>());
        }

        // ✅ FILTER
        public async Task<List<AppointmentDto>> GetAppointmentsByPatientAndDoctor(
            int? patientId, int? doctorId)
        {
            if (!patientId.HasValue && !doctorId.HasValue)
                throw new AppointmentRuleException("Either patient id or doctor id must be provided.");

            var appointments = await _repo.GetByPatientAndDoctor(patientId, doctorId);

            return _mapper.Map<List<AppointmentDto>>(
                appointments ?? new List<Appointment>());
        }
        private async Task LoadNavigation(Appointment appointment)
        {
            if (appointment.Patient == null)
            {
                appointment.Patient = await _patientRepository.getbyidAsync(appointment.PatientId);
            }

            if (appointment.Doctor == null)
            {
                appointment.Doctor = await _doctorRepository.getbyidAsync(appointment.DoctorId);
            }
        }


    }
}
