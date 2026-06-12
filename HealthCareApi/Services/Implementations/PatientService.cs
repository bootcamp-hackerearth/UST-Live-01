using HealthCare.Shared;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HealthCareApi.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task<PagedResult<Patient>> GetPaginatedPatientAsync(
            string searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            return await _patientRepository.GetPaginatedPatientsAsync(
                searchTerm,
                pageNumber,
                pageSize);
        }

        public async Task<Patient> AddPatientAsync(Patient patient)
        {
            patient.IsActive = true;
            await _patientRepository.AddAsync(patient);
            return patient;
        }

        public async Task<Patient> UpdatePatientAsync(Patient updatedPatient)
        {
            var existingPatient = await _patientRepository.GetByIdAsync(updatedPatient.PatientId);

            if (existingPatient == null)
                return null;

            // Controlled update
            existingPatient.FullName = updatedPatient.FullName;
            existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
            existingPatient.Gender = updatedPatient.Gender;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.IsActive = true;

            await _patientRepository.UpdateAsync(existingPatient);

            return existingPatient;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                return false;

            //Soft delete

            patient.IsActive = false;

            await _patientRepository.UpdateAsync(patient);

            return true;
        }
    }
}