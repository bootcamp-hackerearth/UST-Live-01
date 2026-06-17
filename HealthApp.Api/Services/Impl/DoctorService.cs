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

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(data);
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return null;

            return _mapper.Map<DoctorDto>(data);
        }

        public async Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto)
        {
            var entity = _mapper.Map<Doctor>(dto);

            var result = await _repo.Add(entity);
            return _mapper.Map<DoctorDto>(result);
        }

        public async Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorCreateDto dto)
        {
            var entity = _mapper.Map<Doctor>(dto);

            var updated = await _repo.Update(id, entity);
            if (updated == null) return null;

            return _mapper.Map<DoctorDto>(updated);
        }

        public async Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialization,
            bool? isActive)
        {
            var data = await _repo.SearchDoctorsAsync(search, specialization, isActive);
            return _mapper.Map<IEnumerable<DoctorDto>>(data);
        }

        public async Task<bool> ChangeDoctorStatusAsync(int id, bool isActive)
        {
            return await _repo.ChangeStatusAsync(id, isActive);
        }
    }
}