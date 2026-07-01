using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(
            IAppointmentRepository repo,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _repo = repo;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<object> Add(AppointmentDto dto, string identityUserId)
        {
            var patient = await _patientRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new Exception("Patient not found");

            var appointment = new Appointment
            {
                DoctorId = dto.DoctorId,
                PatientId = patient.PatientId,
                ScheduledDate = dto.ScheduledDate,
                TimeSlot = dto.TimeSlot,
                Status = "Pending"
            };


            var isBooked = await _repo.IsSlotBookedAsync(dto.DoctorId,
                dto.ScheduledDate,dto.TimeSlot);

            if (isBooked)
                throw new AppointmentRuleException("Slot already booked");

            await _repo.addAsync(appointment);
            return new { message = "Appointment booked successfully" };
        }


        public async Task<AppointmentDto> GetAppointmentById(int id)
        {
            var a = await _repo.getbyidAsync(id);

            if (a == null)
                throw new EntityNotFoundException("Appointment", id);

            await LoadNavigation(a);

            return _mapper.Map<AppointmentDto>(a);
        }

        public async Task<(List<AppointmentDto> Items, int TotalCount)>
            GetPagedAppointments(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new AppointmentRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new AppointmentRuleException("Invalid page size");

            var (items, total) =
                await _repo.GetPagedAppointmentsAsync(pageNumber, pageSize);

            return (_mapper.Map<List<AppointmentDto>>(items), total);
        }

        public async Task<(List<AppointmentDto> Items, int TotalCount)>
            GetAppointmentsByPatientAndDoctorPaged(
                int? patientId,
                int? doctorId,
                int pageNumber,
                int pageSize)
        {
            if (!patientId.HasValue && !doctorId.HasValue)
                throw new AppointmentRuleException("Provide patient or doctor");

            if (pageNumber <= 0)
                throw new AppointmentRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new AppointmentRuleException("Invalid page size");

            var (items, total) =
                await _repo.GetByPatientAndDoctor(
                    patientId,
                    doctorId,
                    pageNumber,
                    pageSize);

            return (_mapper.Map<List<AppointmentDto>>(items), total);
        }

        public async Task<AppointmentDto> CancelAppointment(int id, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new AppointmentRuleException("Reason required");

            var saved = await _repo.CancelAppointmentAsync(id, reason);

            if (saved == null)
                throw new EntityNotFoundException("Appointment", id);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto> ConfirmAppointment(int id)
        {
            var saved = await _repo.UpdateStatusAsync(id, "Confirmed");

            if (saved == null)
                throw new EntityNotFoundException("Appointment", id);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto> CompleteAppointment(int id)
        {
            var saved = await _repo.UpdateStatusAsync(id, "Completed");

            if (saved == null)
                throw new EntityNotFoundException("Appointment", id);

            return _mapper.Map<AppointmentDto>(saved);
        }

        public async Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            var list = await _repo.GetBookedSlotsAsync(doctorId, date);
            return list ?? new List<string>();
        }

        public async Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot)
        {
            return await _repo.IsSlotBookedAsync(doctorId, date, timeSlot);
        }

        public async Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(
            int doctorId, DateTime from, DateTime to)
        {
            var list = await _repo.GetUpcomingByDoctorAsync(doctorId, from, to);

            return _mapper.Map<List<AppointmentDto>>(list ?? new List<Appointment>());
        }

        private async Task LoadNavigation(Appointment a)
        {
            if (a.Patient == null)
                a.Patient = await _patientRepository.getbyidAsync(a.PatientId);

            if (a.Doctor == null)
                a.Doctor = await _doctorRepository.getbyidAsync(a.DoctorId);
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByUserAsync(string identityUserId)
        {
            var patient = await _patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new EntityNotFoundException("Patient", 0);

            var list = await _repo.GetByPatientIdAsync(patient.PatientId);

            return _mapper.Map<List<AppointmentDto>>(list ?? new List<Appointment>());
        }


        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorAsync(string identityUserId)
        {
            var doctor = await _doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor", 0);

            var list = await _repo.GetByDoctorIdAsync(doctor.DoctorId);

            return _mapper.Map<List<AppointmentDto>>(list ?? new List<Appointment>());
        }
    }
}