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

            var duplicate = await _repo.EmailExistsAsync(patientDto.Email);

            if (duplicate)
                throw new ConflictException("A patient with the same email already exists.");

            var patient = _mapper.Map<Patient>(patientDto);
            var savedPatient = await _repo.addAsync(patient);

            if (savedPatient == null)
                throw new BusinessRuleException("Unable to register patient.");

            return _mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<(List<PatientDto> Items, int TotalCount)>
            GetPagedPatientsAsync(int pageNumber, int pageSize, string? search = null)
        {
            if (pageNumber <= 0)
                throw new BusinessRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new BusinessRuleException("Invalid page size");

            var (items, total) =
                await _repo.GetPagedPatientsAsync(pageNumber, pageSize, search);

            return (_mapper.Map<List<PatientDto>>(items), total);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            ValidatePatientId(id);

            var patient = await _repo.getbyidAsync(id);

            if (patient == null)
                throw new EntityNotFoundException("Patient", id);

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> GetMyProfileAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new BusinessRuleException("Invalid user");

            var patient = await _repo.GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
                throw new BusinessRuleException("Profile not found");

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateMyProfileAsync(string identityUserId, PatientDto patientDto)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
                throw new BusinessRuleException("Invalid user");

            ValidatePatientDto(patientDto);

            var existingPatient = await _repo.GetByIdentityUserIdAsync(identityUserId);

            if (existingPatient == null)
                throw new BusinessRuleException("Profile not found");

            var duplicate = await _repo.EmailExistsAsync(
                patientDto.Email,
                existingPatient.PatientId);

            if (duplicate)
                throw new ConflictException("Email already exists.");

            var patient = _mapper.Map<Patient>(patientDto);
            patient.PatientId = existingPatient.PatientId;
            patient.IdentityUserId = existingPatient.IdentityUserId;

            var updated = await _repo.updateAsync(existingPatient.PatientId, patient);

            if (updated == null)
                throw new BusinessRuleException("Unable to update profile.");

            return _mapper.Map<PatientDto>(updated);
        }

        private void ValidatePatientId(int id)
        {
            if (id <= 0)
                throw new BusinessRuleException("Invalid patient id");
        }

        private void ValidatePatientDto(PatientDto patientDto)
        {
            if (patientDto == null)
                throw new BusinessRuleException("Patient required");

            if (string.IsNullOrWhiteSpace(patientDto.FullName))
                throw new BusinessRuleException("Name required");

            if (string.IsNullOrWhiteSpace(patientDto.Email))
                throw new BusinessRuleException("Email required");
        }
    }
}