using AutoMapper;
using HealthApp.Shared.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Api.Exceptions;

namespace HealthApp.Api.Service.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PatientDto> AddPatientAsync(PatientDto patientDto)
        {
            ValidatePatientDto(patientDto);

            string email = patientDto.Email!;

            var duplicate = await _repo.EmailExistsAsync(email);

            if (duplicate)
                throw new ConflictException(
                    "A patient with the same email already exists.");

            var patient = _mapper.Map<Patient>(patientDto);

            var savedPatient = await _repo.addAsync(patient);

            if (savedPatient == null)
                throw new BusinessRuleException(
                    "Unable to register patient.");

            return _mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<(List<PatientDto> Items, int TotalCount)>
            GetPagedPatientsAsync(
                int pageNumber,
                int pageSize,
                string? search = null)
        {
            if (pageNumber <= 0)
                throw new BusinessRuleException(
                    "Invalid page number");

            if (pageSize <= 0)
                throw new BusinessRuleException(
                    "Invalid page size");

            var (items, total) =
                await _repo.GetPagedPatientsAsync(
                    pageNumber,
                    pageSize,
                    search);

            return (
                _mapper.Map<List<PatientDto>>(items),
                total
            );
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            ValidatePatientId(id);

            var patient = await _repo.getbyidAsync(id);

            if (patient == null)
                throw new EntityNotFoundException(
                    "Patient",
                    id);

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> GetMyProfileAsync(
            string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new BusinessRuleException(
                    "Invalid user");

            var patient =
                await _repo.GetByIdentityUserIdAsync(
                    identityUserId);

            if (patient == null)
                throw new BusinessRuleException(
                    "Profile not found");

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateMyProfileAsync(
            string identityUserId,
            PatientUpdateDto patientDto)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new BusinessRuleException(
                    "Invalid user");

            ValidatePatientUpdateDto(patientDto);

            var existingPatient =
                await _repo.GetByIdentityUserIdAsync(
                    identityUserId);

            if (existingPatient == null)
                throw new BusinessRuleException(
                    "Profile not found");

            // ✅ Update only editable profile fields
            existingPatient.FullName = patientDto.FullName.Trim();
            existingPatient.PhoneNumber = patientDto.PhoneNumber.Trim();
            if (!patientDto.DateOfBirth.HasValue)
                throw new BusinessRuleException("Date of birth is required");

            existingPatient.DateOfBirth = patientDto.DateOfBirth.Value.Date;
            existingPatient.InsuranceId = string.IsNullOrWhiteSpace(patientDto.InsuranceId)
                ? null
                : patientDto.InsuranceId.Trim();

            var updated =
                await _repo.updateAsync(
                    existingPatient.PatientId,
                    existingPatient);

            if (updated == null)
                throw new BusinessRuleException(
                    "Unable to update profile.");

            return _mapper.Map<PatientDto>(updated);
        }

        private static void ValidatePatientId(int id)
        {
            if (id <= 0)
                throw new BusinessRuleException(
                    "Invalid patient id");
        }

        private static void ValidatePatientDto(
            PatientDto patientDto)
        {
            if (patientDto == null)
                throw new BusinessRuleException(
                    "Patient required");

            if (string.IsNullOrWhiteSpace(
                patientDto.FullName))
                throw new BusinessRuleException(
                    "Name required");

            if (string.IsNullOrWhiteSpace(
                patientDto.Email))
                throw new BusinessRuleException(
                    "Email required");
        }

        private static void ValidatePatientUpdateDto(
            PatientUpdateDto patientDto)
        {
            if (patientDto == null)
                throw new BusinessRuleException(
                    "Patient update data required");

            if (string.IsNullOrWhiteSpace(patientDto.FullName))
                throw new BusinessRuleException(
                    "Name required");

            if (string.IsNullOrWhiteSpace(patientDto.PhoneNumber))
                throw new BusinessRuleException(
                    "Phone number required");

            if (!patientDto.DateOfBirth.HasValue)
                throw new BusinessRuleException("Date of birth is required");

        }
    }
}