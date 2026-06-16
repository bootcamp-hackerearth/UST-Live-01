using AutoMapper;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
namespace HealthApp.API.Service.Impl
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<PatientDto> AddPatientAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = await repository.AddAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);
        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(await repository.GetAllAsync());
        }

        public async Task<PatientDto> GetByIdAsync(int id)
        {
            return mapper.Map<PatientDto>(await repository.GetByIdAsync(id));
        }

        public async Task<PatientDto> UpdatePatientAsync(int id, PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            patient.PatientId = id;
            var updated = await repository.UpdateAsync(id, patient);
            return mapper.Map<PatientDto>(updated);
        }
    }
}
