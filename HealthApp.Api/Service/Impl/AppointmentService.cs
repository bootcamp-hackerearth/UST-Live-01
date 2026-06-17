using AutoMapper;
using HealthApp.Api.Dto;
using HealthApp.Api.Repository.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HospitalManagementAPI.Model;

namespace HealthApp.Api.Service.Impl
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository _repo;
        private IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo,IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }
        public async Task<AppointmentDto> Add(AppointmentDto dto)
        {
            var a=_mapper.Map<Appointment>(dto);
            var saveda=await _repo.addasync(a);
            return _mapper.Map<AppointmentDto>(saveda);
        }

        public async Task<AppointmentDto> CancelAppointment(int appointmentId, string reason)
        {
            var a= _mapper.Map<Appointment>(appointmentId);
            var saveda=await _repo.CancelAppointmentAsync(appointmentId, reason);
            return _mapper.Map<AppointmentDto>(saveda);

        }

        public Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date)
        {
            var a= _mapper.Map<Appointment>(doctorId);
            var saveda = _repo.GetBookedSlotsAsync(doctorId, date);
            return _mapper.Map<Task<List<string>>>(saveda);
        }

        public Task<AppointmentDto> CompleteAppointment(int appointmentId)
        {
            var a = _mapper.Map<Appointment>(appointmentId);
            var saveda = _repo.UpdateStatusAsync(appointmentId, "Completed");
            return _mapper.Map<Task<AppointmentDto>>(saveda);
        }

        public Task<AppointmentDto> ConfirmAppointment(int appointmentId)
        {
            var a = _mapper.Map<Appointment>(appointmentId);
            var saveda = _repo.UpdateStatusAsync(appointmentId, "Confirmed");
            return _mapper.Map<Task<AppointmentDto>>(saveda);
        }

        public Task<List<AppointmentDto>> GetAllAppointments()
        {
            var saveda = _repo.getallasync();
            return _mapper.Map<Task<List<AppointmentDto>>>(saveda);
        }

        public Task<AppointmentDto> GetAppointmentById(int id)
        {
            var a = _mapper.Map<Appointment>(id);
            var saveda = _repo.getbyidasync(id);
            return _mapper.Map<Task<AppointmentDto>>(saveda);
        }

        public Task<List<AppointmentDto>> GetAppointmentsByPatient(int patientId)
        {
           var a = _mapper.Map<Appointment>(patientId);
            var saveda = _repo.GetByPatientAsync(patientId);
            return _mapper.Map<Task<List<AppointmentDto>>>(saveda);
        }

        public Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(int doctorId, DateTime fromDate, DateTime toDate)
        {
           var a = _mapper.Map<Appointment>(doctorId);
            var saveda = _repo.GetUpcomingByDoctorAsync(doctorId, fromDate, toDate);
            return _mapper.Map<Task<List<AppointmentDto>>>(saveda);
        }
    }
}
