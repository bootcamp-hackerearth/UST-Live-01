using AutoMapper;
using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.DTO.PatientDtos;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;

namespace HealthAxis.API.Services.Implementation
{
    public class PatientService(
        IPatientRepository patientRepository,
        IHealthRecordRepository healthRecordRepository,
        IMapper mapper) : IPatientService
    {
        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(
                await patientRepository.GetAllAsync());
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdateAsync(int id,PatientDto patientDto)
        {
            var existingPatient = await patientRepository.GetByIdAsync(id);

            if (existingPatient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var patient = mapper.Map<Patient>(patientDto);
            patient.PatientId = id;

            var updated =await patientRepository.UpdateAsync(id, patient);

            return mapper.Map<PatientDto>(updated);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId)
        {
            var patient =await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var healthRecords = await healthRecordRepository.GetAllAsync();

            var patientHealthRecords =healthRecords .Where(record => record.PatientId == patientId) .ToList();

            return mapper.Map<List<HealthRecordDto>>(patientHealthRecords);
        }
    }
}