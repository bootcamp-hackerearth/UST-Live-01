using AutoMapper;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.DTO.PatientDto;

namespace HealthAxis.API.Services.Implementation
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);

            var savedEntity = await repository.AddAsync(patient);

            return mapper.Map<PatientDto>(savedEntity);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(await repository.GetAllAsync());
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            return mapper.Map<PatientDto>(await repository.GetByIdAsync(id));
        }

        public async Task<PatientDto?> UpdateAsync(int id, PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);

            var updated = await repository.UpdateAsync(id, patient);

            return mapper.Map<PatientDto>(updated);
        }
    }
}