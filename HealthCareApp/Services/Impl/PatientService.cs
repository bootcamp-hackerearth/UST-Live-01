using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;

namespace HealthCareApp.Services
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await repository.GetAllAsync();

            return mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int patientId)
        {
            var patient = await repository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new Exception("Patient not found.");
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
        {
            if (dto.DateOfBirth > DateTime.Today)
            {
                throw new Exception("Date of birth cannot be a future date.");
            }

            var patient = mapper.Map<Patient>(dto);

            var savedPatient = await repository.CreateAsync(patient);

            return mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<PatientDto> UpdatePatientAsync(int patientId, UpdatePatientDto dto)
        {
            if (dto.DateOfBirth > DateTime.Today)
            {
                throw new Exception("Date of birth cannot be a future date.");
            }

            var patient = mapper.Map<Patient>(dto);

            patient.PatientId = patientId;

            var updatedPatient = await repository.UpdateAsync(patientId, patient);

            if (updatedPatient is null)
            {
                throw new Exception("Patient not found.");
            }

            return mapper.Map<PatientDto>(updatedPatient);
        }

        public async Task<PatientDto> DeletePatientAsync(int patientId)
        {
            var deletedPatient = await repository.DeleteAsync(patientId);

            if (deletedPatient is null)
            {
                throw new Exception("Patient not found.");
            }

            return mapper.Map<PatientDto>(deletedPatient);
        }
    }
}