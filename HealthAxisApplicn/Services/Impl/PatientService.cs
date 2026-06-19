using AutoMapper;
using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {

        public async Task<PatientDto> CreateAsync(CreatePatientDto entity)
        {
            var patient = mapper.Map<Patient>(entity);
            var savedEntity = await repository.CreateAsync(patient);
            return mapper.Map<PatientDto>(savedEntity);

        }

        public async Task<PatientDto> DeactivatePatientAsync(int id)
        {
            var existing = await repository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Patient Not Found");
            if (!existing.IsActive)
                throw new Exception("Cannot update inactive patient");

            existing.IsActive = false;

            var updated = await repository.UpdateAsync(id, existing);

            return mapper.Map<PatientDto>(updated);

        }

        public async Task<List<PatientDto>> GetAllAsync()
        {
            var patients = await repository.GetAllAsync();
            return mapper.Map<List<PatientDto>>(patients);
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

        public async Task<List<PatientDto>> SearchByPatientNameAsync(string name)
        {
            var patient = await repository.SearchByNameAsync(name);
            return mapper.Map<List<PatientDto>>(patient); 
        }

        public async Task<PatientDto?> SearchByPhoneNumberAsync(string phoneNumber)
        {
            var patient = await repository.SearchByPhoneNumberAsync(phoneNumber);
            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdateAsync(int id, CreatePatientDto entity)
        {

            var existing = await repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Patient Not Found");

            existing.PatientName = entity.PatientName;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.Gender = entity.Gender;
            existing.Email = entity.Email;
            existing.PhoneNo = entity.PhoneNo;
            existing.InsuranceID = entity.InsuranceID;

            var updated = await repository.UpdateAsync(id, existing);

            return mapper.Map<PatientDto>(updated);

        }
    }
}
