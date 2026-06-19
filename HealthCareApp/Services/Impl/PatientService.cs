using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Exceptions;
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
            ValidatePatientId(patientId);

            var patient = await repository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
        {
            ValidateCreatePatientDto(dto);

            string patientName = dto.FullName.Trim().ToLower();
            string email = dto.Email.Trim().ToLower();
            string phoneNumber = dto.PhoneNumber.Trim();
            DateTime dateOfBirth = dto.DateOfBirth.Date;

            bool duplicate = await repository.IsDuplicatePatientAsync(
                patientName,
                email,
                phoneNumber,
                dateOfBirth);

            if (duplicate)
            {
                throw new ConflictException("A patient with similar details already exists.");
            }

            var patient = mapper.Map<Patient>(dto);

            patient.DateOfBirth = dateOfBirth;
            patient.CreatedDate = DateTime.Now;

            var savedPatient = await repository.CreateAsync(patient);

            return mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<PatientDto> UpdatePatientAsync(int patientId, UpdatePatientDto dto)
        {
            ValidatePatientId(patientId);

            ValidateUpdatePatientDto(dto);

            var existingPatient = await repository.GetByIdAsync(patientId);

            if (existingPatient is null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            string patientName = dto.FullName.Trim().ToLower();
            string email = dto.Email.Trim().ToLower();
            string phoneNumber = dto.PhoneNumber.Trim();
            DateTime dateOfBirth = dto.DateOfBirth.Date;

            bool duplicate = await repository.IsDuplicatePatientAsync(
                patientName,
                email,
                phoneNumber,
                dateOfBirth,
                patientId);

            if (duplicate)
            {
                throw new ConflictException("Another patient with similar details already exists.");
            }

            var patient = mapper.Map<Patient>(dto);

            patient.PatientId = patientId;
            patient.DateOfBirth = dateOfBirth;
            patient.CreatedDate = existingPatient.CreatedDate;

            var updatedPatient = await repository.UpdateAsync(patientId, patient);

            if (updatedPatient is null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return mapper.Map<PatientDto>(updatedPatient);
        }

        private void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid patient reference.");
            }
        }

        private void ValidateCreatePatientDto(CreatePatientDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            ValidatePatientCommonFields(
                dto.FullName,
                dto.DateOfBirth,
                dto.Email,
                dto.PhoneNumber);
        }

        private void ValidateUpdatePatientDto(UpdatePatientDto dto)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            ValidatePatientCommonFields(
                dto.FullName,
                dto.DateOfBirth,
                dto.Email,
                dto.PhoneNumber);
        }

        private void ValidatePatientCommonFields(
            string patientName,
            DateTime dateOfBirth,
            string email,
            string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(patientName))
            {
                throw new BusinessRuleException("Patient name is required.");
            }

            if (dateOfBirth.Date < new DateTime(1900, 1, 1))
            {
                throw new BusinessRuleException("Date of birth cannot be before 01 Jan 1900.");
            }

            if (dateOfBirth.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Date of birth cannot be a future date.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BusinessRuleException("Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new BusinessRuleException("Phone number is required.");
            }
        }

        public async Task<PatientDto> GetMyProfileAsync(string identityUserId)
{
    if (string.IsNullOrWhiteSpace(identityUserId))
    {
        throw new BusinessRuleException("Invalid logged-in user.");
    }

    var patient = await repository.GetByIdentityUserIdAsync(identityUserId);

    if (patient is null)
    {
        throw new EntityNotFoundException("Patient profile for logged-in user", 0);
    }

    return mapper.Map<PatientDto>(patient);
}

public async Task<PatientDto> UpdateMyProfileAsync(string identityUserId, UpdatePatientDto dto)
{
    if (string.IsNullOrWhiteSpace(identityUserId))
    {
        throw new BusinessRuleException("Invalid logged-in user.");
    }

    if (dto is null)
    {
        throw new BusinessRuleException("Patient details are required.");
    }

    if (dto.DateOfBirth.Date > DateTime.Today)
    {
        throw new BusinessRuleException("Date of birth cannot be a future date.");
    }

    var existingPatient = await repository.GetByIdentityUserIdAsync(identityUserId);

    if (existingPatient is null)
    {
        throw new EntityNotFoundException("Patient profile for logged-in user", 0);
    }

    var patient = mapper.Map<Patient>(dto);

    patient.PatientId = existingPatient.PatientId;
    patient.IdentityUserId = existingPatient.IdentityUserId;
    patient.Email = existingPatient.Email;
    patient.CreatedDate = existingPatient.CreatedDate;

    var updatedPatient = await repository.UpdateAsync(existingPatient.PatientId, patient);

    if (updatedPatient is null)
    {
        throw new EntityNotFoundException("Patient", existingPatient.PatientId);
    }

    return mapper.Map<PatientDto>(updatedPatient);
}
    }
}