using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;

namespace HealthApp.Api.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(data);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return null;

            return _mapper.Map<PatientDto>(data);
        }

        public async Task<PatientDto> CreatePatientAsync(PatientCreateDto dto)
        {
            var entity = _mapper.Map<Patient>(dto);

            var isDuplicate = await _repo.IsDuplicatePatient(
                entity.FullName!,
                entity.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                entity.Email!
            );

            if (isDuplicate)
                throw new Exception("Duplicate patient");

            var result = await _repo.Add(entity);
            return _mapper.Map<PatientDto>(result);
        }

        public async Task<PatientDto?> UpdatePatientAsync(int id, PatientCreateDto dto)
        {
            var entity = _mapper.Map<Patient>(dto);

            var updated = await _repo.Update(id, entity);
            if (updated == null) return null;

            return _mapper.Map<PatientDto>(updated);
        }

        public async Task<IEnumerable<PatientDto>> SearchPatientsAsync(string? name, string? email)
        {
            var data = await _repo.GetPatientsAsync(name, email);
            return _mapper.Map<IEnumerable<PatientDto>>(data);
        }
    }
}