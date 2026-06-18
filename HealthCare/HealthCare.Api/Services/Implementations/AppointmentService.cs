using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.Appointments;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<Appointment> _repository;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;

        public AppointmentService(IRepository<Appointment> repository, IMapper mapper, HealthCareDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddAsync(CreateAppointmentDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _repository.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            _mapper.Map(dto, appointment);

            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<AppointmentListDto?> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentListDto?>(appointment);
        }

        public async Task<IEnumerable<AppointmentListDto>> GetAllAsync()
        {
            var appointments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentListDto>>(appointments);

        }
    }
}
