using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid patient id is required.");
            }

            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", id);
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task RegisterPatientAsync(PatientCreateDto dto)
        {
            if (dto == null)
            {
                throw new InvalidRequestException("Patient data is required.");
            }

            ValidatePatient(dto);

            if (dto.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException("Future date is not allowed.");
            }

            string email = dto.Email.Trim();

            var patientsWithEmail = await _patientRepository.GetPatientsAsync(null, email);

            bool emailExists = patientsWithEmail.Any(p =>
                string.Equals(p.Email, email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new DuplicateEntityException(
                    "A patient with this email already exists.");
            }

            bool duplicatePatient = await _patientRepository.IsDuplicatePatient(
                dto.FullName.Trim(),
                dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                email);

            if (duplicatePatient)
            {
                throw new DuplicateEntityException(
                    "A patient with same name, date of birth and email already exists.");
            }

            var patient = _mapper.Map<Patient>(dto);

            patient.FullName = dto.FullName.Trim();
            patient.DateOfBirth = (DateOnly)dto.DateOfBirth!;
            patient.Gender = dto.Gender.Trim();
            patient.PhoneNumber = dto.PhoneNumber.Trim();
            patient.Email = email;
            patient.InsuranceId = dto.InsuranceId?.Trim();
            patient.CreatedDate = DateTime.Now;

            await _patientRepository.Add(patient);
        }

        public async Task UpdatePatientAsync(int id, PatientCreateDto dto)
        {
            if (id <= 0)
            {
                throw new InvalidRequestException("Valid patient id is required.");
            }

            if (dto == null)
            {
                throw new InvalidRequestException("Patient data is required.");
            }

            ValidatePatient(dto);

            if (dto.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new BusinessRuleViolationException("Future date is not allowed.");
            }

            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", id);
            }

            string email = dto.Email.Trim();

            var patientsWithEmail = await _patientRepository.GetPatientsAsync(null, email);

            bool emailUsedByAnotherPatient = patientsWithEmail.Any(p =>
                p.PatientId != id &&
                string.Equals(p.Email, email, StringComparison.OrdinalIgnoreCase));

            if (emailUsedByAnotherPatient)
            {
                throw new DuplicateEntityException(
                    "Another patient already uses this email.");
            }

            patient.FullName = dto.FullName.Trim();
            patient.DateOfBirth = (DateOnly)dto.DateOfBirth!;
            patient.Gender = dto.Gender.Trim();
            patient.PhoneNumber = dto.PhoneNumber.Trim();
            patient.Email = email;
            patient.InsuranceId = dto.InsuranceId?.Trim();

            await _patientRepository.Update(id, patient);
        }

        public async Task<IEnumerable<PatientDto>> SearchPatientsAsync(
            string? name,
            string? email)
        {
            var patients = await _patientRepository.GetPatientsAsync(name, email);

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        private static void ValidatePatient(PatientCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new InvalidRequestException("Patient name is required.");
            }

            if (dto.DateOfBirth == default)
            {
                throw new InvalidRequestException("Date of birth is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new InvalidRequestException("Gender is required.");
            }

            string gender = dto.Gender.Trim();

            if (gender != "Male" && gender != "Female" && gender != "Other")
            {
                throw new InvalidRequestException("Invalid gender.");
            }

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                throw new InvalidRequestException("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new InvalidRequestException("Email is required.");
            }
        }
    }
}