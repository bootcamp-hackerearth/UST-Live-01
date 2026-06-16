using S3_HealthAxisApi.DTOs.Patient;
using S3_HealthAxisApi.Models;
using S3_HealthAxisApi.Repository.Interface;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientDto>> GetAllAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            return patients.Select(MapToPatientDto);
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            return patient == null
                ? null
                : MapToPatientDto(patient);
        }

        public async Task<IEnumerable<PatientSearchResultDto>> SearchByNameAsync(string name)
        {
            var patients = await _patientRepository.SearchByNameAsync(name);

            return patients.Select(p => new PatientSearchResultDto
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                IsActive = p.IsActive
            });
        }

        public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
        {
            ValidatePatient(dto);

            var patient = new Patient
            {
                FullName = dto.FullName.Trim(),
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber.Trim(),
                Email = dto.Email?.Trim()??String.Empty,
                InsuranceNumber = dto.InsuranceNumber?.Trim(),
                IsActive = true
            };

            await _patientRepository.AddAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return MapToPatientDto(patient);
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto)
        {
            ValidatePatient(dto);

            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                throw new KeyNotFoundException($"Patient with Id {id} not found.");

            patient.FullName = dto.FullName.Trim();
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.PhoneNumber = dto.PhoneNumber.Trim();
            patient.Email = dto.Email?.Trim() ?? String.Empty;
            patient.InsuranceNumber = dto.InsuranceNumber?.Trim();

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();
        }

        public async Task DeactivateAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                throw new KeyNotFoundException($"Patient with Id {id} not found.");

            patient.IsActive = false;

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();
        }

        public async Task ActivateAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
                throw new KeyNotFoundException($"Patient with Id {id} not found.");

            patient.IsActive = true;

            await _patientRepository.UpdateAsync(patient);
            await _patientRepository.SaveChangesAsync();
        }

        private static void ValidatePatient(CreatePatientDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Patient name is required.");

            if (dto.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Date of birth cannot be in the future.");

            if (dto.DateOfBirth < DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
                throw new ArgumentException("Invalid date of birth.");

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new ArgumentException("Phone number is required.");
        }

        private static void ValidatePatient(UpdatePatientDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Patient name is required.");

            if (dto.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Date of birth cannot be in the future.");

            if (dto.DateOfBirth < DateOnly.FromDateTime(DateTime.Today.AddYears(-120)))
                throw new ArgumentException("Invalid date of birth.");

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new ArgumentException("Phone number is required.");
        }

        private static PatientDto MapToPatientDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceId = patient.InsuranceNumber,
                IsActive = patient.IsActive
            };
        }
    }
}