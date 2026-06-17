using AutoMapper;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Models.Dto;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {

        public async Task<PatientDto> CreateAsync(PatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = await repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);

        }

        public async Task<PatientDto> DeactivatePatientAsync(int id)
        {
            var patient = mapper.Map<Patient>(await repository.GetByIdAsync(id));
            patient.IsActive = false;
            return mapper.Map<PatientDto>(patient);
        }

        public async Task<List<PatientDto?>> GetAllAsync()
        {
            var patients = await repository.GetAllAsync();
            return mapper.Map<List<PatientDto?>>(patients);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await repository.GetByIdAsync(id);
            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> SearchByEmailAsync(string email)
        {
            var patient = await repository.SearchByEmailAsync(email);
            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> SearchByPatientNameAsync(string name)
        {
            var patient = await repository.SearchByPatientNameAsync(name);
            return mapper.Map<PatientDto>(patient); 
        }

        public async Task<PatientDto?> SearchByPhoneNumberAsync(string phoneNumber)
        {
            var patient = await repository.SearchByPhoneNumberAsync(phoneNumber);
            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdatebyAsync(int id, PatientDto entity)
        {
            var newPatient = mapper.Map<Patient>(entity);
            var updatedPatient = await repository.UpdatebyAsync(id, newPatient);
            return mapper.Map<PatientDto>(updatedPatient);

        }
    }
}
