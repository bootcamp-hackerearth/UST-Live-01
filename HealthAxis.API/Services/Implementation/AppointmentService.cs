using AutoMapper;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.AppointmentDto;

namespace HealthAxis.API.Services.Implementation
{
    public class AppointmentService( IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            return mapper.Map<AppointmentDto>(await repository.GetByIdAsync(id));
        }

        public async Task<AppointmentDto> AddAsync(AppointmentDto appointmentDto)
        {
            var appointment = mapper.Map<Appointment>(appointmentDto);

            var saved = await repository.AddAsync(appointment);

            return mapper.Map<AppointmentDto>(saved);
        }

        public async Task<AppointmentDto?> UpdateAsync(int id, AppointmentDto appointmentDto)
        {
            var appointment =mapper.Map<Appointment>(appointmentDto);

            appointment.AppointmentId = id;

            var updated = await repository.UpdateAsync(id, appointment);

            return mapper.Map<AppointmentDto>(updated);
        }

        public async Task<AppointmentDto?> DeleteAsync(int id)
        {
            var deleted = await repository.DeleteAsync(id);

            return mapper.Map<AppointmentDto>(deleted);
        }
    }
}