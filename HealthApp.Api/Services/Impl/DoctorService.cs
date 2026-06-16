using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;

namespace HealthApp.Api.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(CancellationToken ct)
        {
            var data = await _repo.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<DoctorDto>>(data);
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id, CancellationToken ct)
        {
            var data = await _repo.GetByIdAsync(id, ct);
            if (data == null) return null;

            return _mapper.Map<DoctorDto>(data);
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Doctor>(dto);

            var result = await _repo.Add(entity, ct);
            return _mapper.Map<DoctorDto>(result);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorCreateDto dto, CancellationToken ct)
        {
            var entity = _mapper.Map<Doctor>(dto);

            var updated = await _repo.Update(id, entity, ct);
            if (updated == null) return null;

            return _mapper.Map<DoctorDto>(updated);
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialization,
            bool? isActive,
            CancellationToken ct)
        {
            var data = await _repo.SearchDoctorsAsync(search, specialization, isActive, ct);
            return _mapper.Map<IEnumerable<DoctorDto>>(data);
        }

        public async Task<bool> ChangeDoctorStatusAsync(int id, bool isActive, CancellationToken ct)
        {
            return await _repo.ChangeStatusAsync(id, isActive, ct);
        }
    }
}
