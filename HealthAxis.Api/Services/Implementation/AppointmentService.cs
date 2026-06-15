using AutoMapper;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.DTOs;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = await repository.CreateAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
        }

        public async Task<AppointmentDto> DeleteAsync(int id)
        {
            var deleted = await repository.DeleteAsync(id);
            return mapper.Map<AppointmentDto>(deleted);
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            return mapper.Map<AppointmentDto>(await repository.GetByIdAsync(id));
        }

        public async Task<AppointmentDto> UpdateAsync(int id, AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            appointment.AppointmentId = id;
            var updated = await repository.UpdateAsync(id, appointment);
            return mapper.Map<AppointmentDto> (updated);
        }
    }
}
