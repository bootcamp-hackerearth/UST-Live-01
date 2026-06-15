

using AutoMapper;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.DTOs;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddPatientAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = await repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(await  repository.GetAllAsync());
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            return mapper.Map<PatientDto>(await repository.GetByIdAsync(id));
        }

        public async Task<PatientDto> UpdatePatientAsync(int id,PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            patient.PatientId = id;
            var updated = await repository.UpdateAsync(id,patient);
            return mapper.Map<PatientDto>(updated);
        }
    }
}
