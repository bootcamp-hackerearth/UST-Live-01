using AutoMapper;
using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> CreateAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = await repository.CreateAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
        }

        public async Task<AppointmentDto?> DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await repository.DeleteAppointmentAsync(appointmentId);
            return mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<List<AppointmentDto?>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto?>>(await repository.GetAllAsync());
        }

        public async Task<List<AppointmentDto>> GetAppointmentByDoctorIdAsync(int doctorId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentByDoctorIdAsync(doctorId));
        }

        public async Task<List<AppointmentDto>> GetAppointmentByPatientIdAsync(int patientId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentByPatientIdAsync(patientId));
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await repository.GetByIdAsync(id);
            return mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<AppointmentDto?> UpdatebyAsync(int id, AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var updatedAppointment = await repository.UpdatebyAsync(id, appointment);
            return mapper.Map<AppointmentDto?>(updatedAppointment);
        }
    }
}
