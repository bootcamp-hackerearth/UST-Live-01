using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;

namespace HealthApp.Api.Services.Impl
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

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken ct)
        {
            var data = await _repo.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<AppointmentDto>>(data);
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id, CancellationToken ct)
        {
            var data = await _repo.GetByIdAsync(id, ct);
            if (data == null) return null;

            return _mapper.Map<AppointmentDto>(data);
        }

        public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Appointment>(dto);

            var result = await _repo.Add(entity, ct);
            return _mapper.Map<AppointmentDto>(result);
        }
    }
}
