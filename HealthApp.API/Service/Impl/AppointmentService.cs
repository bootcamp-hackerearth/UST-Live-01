using AutoMapper;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
namespace HealthApp.API.Service.Impl
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> AddAsync(AppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = await repository.AddAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
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
            return mapper.Map<AppointmentDto>(updated);
        }
    }
}
