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

            var existingPatients = await _repo.getallAsync();

            bool duplicate = existingPatients != null &&
                             existingPatients.Any(p =>
                                 !string.IsNullOrWhiteSpace(p.Email) &&
                                 p.Email.Trim().ToLower() == patientDto.Email.Trim().ToLower());

            if (duplicate)
            {
                throw new ConflictException("A patient with the same email already exists.");
            }

            var patient = _mapper.Map<Patient>(patientDto);
            var savedPatient = await _repo.addAsync(patient);

            if (savedPatient == null)
            {
                throw new BusinessRuleException("Unable to register patient at the moment.");
            }

            return _mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _repo.getallAsync();

            if (patients == null)
            {
                return new List<PatientDto>();
            }

            return _mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            ValidatePatientId(id);

            var patient = await _repo.getbyidAsync(id);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", id);
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdatePatientByIdAsync(int id, PatientDto patientDto)
        {
            ValidatePatientId(id);
            ValidatePatientDto(patientDto);

            var existingPatient = await _repo.getbyidAsync(id);

            if (existingPatient == null)
            {
                throw new EntityNotFoundException("Patient", id);
            }

            var allPatients = await _repo.getallAsync();

            bool duplicate = allPatients != null &&
                             allPatients.Any(p =>
                                 p.PatientId != id &&
                                 !string.IsNullOrWhiteSpace(p.Email) &&
                                 p.Email.Trim().ToLower() == patientDto.Email.Trim().ToLower());

            if (duplicate)
            {
                throw new ConflictException("Another patient with the same email already exists.");
            }

            var patient = _mapper.Map<Patient>(patientDto);
            patient.PatientId = id;
            patient.IdentityUserId = existingPatient.IdentityUserId; // preserve link

            var updatedPatient = await _repo.updateAsync(id, patient);

            if (updatedPatient == null)
            {
                throw new EntityNotFoundException("Patient", id);
            }

            return _mapper.Map<PatientDto>(updatedPatient);
        }

        public async Task<PatientDto> GetMyProfileAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged in user.");
            }

            var patient = await _repo.GetByIdentityUserIdAsync(identityUserId);

            if (patient == null)
            {
                throw new BusinessRuleException("Patient profile not found for this logged-in user.");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateMyProfileAsync(string identityUserId, PatientDto patientDto)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged in user.");
            }

            ValidatePatientDto(patientDto);

            var existingPatient = await _repo.GetByIdentityUserIdAsync(identityUserId);

            if (existingPatient == null)
            {
                throw new BusinessRuleException("Patient profile not found for this logged-in user.");
            }

            var allPatients = await _repo.getallAsync();

            bool duplicate = allPatients != null &&
                             allPatients.Any(p =>
                                 p.PatientId != existingPatient.PatientId &&
                                 !string.IsNullOrWhiteSpace(p.Email) &&
                                 p.Email.Trim().ToLower() == patientDto.Email.Trim().ToLower());

            if (duplicate)
            {
                throw new ConflictException("Another patient with the same email already exists.");
            }

            var patient = _mapper.Map<Patient>(patientDto);
            patient.PatientId = existingPatient.PatientId;
            patient.IdentityUserId = existingPatient.IdentityUserId; // preserve link

            var updatedPatient = await _repo.updateAsync(existingPatient.PatientId, patient);

            if (updatedPatient == null)
            {
                throw new BusinessRuleException("Unable to update patient profile.");
            }

            return _mapper.Map<PatientDto>(updatedPatient);
        }

        private void ValidatePatientId(int id)
        {
            if (id <= 0)
            {
                throw new BusinessRuleException("Please provide a valid patient reference.");
            }
        }

        private void ValidatePatientDto(PatientDto patientDto)
        {
            if (patientDto == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            if (string.IsNullOrWhiteSpace(patientDto.FullName))
            {
                throw new BusinessRuleException("Patient name is required.");
            }

            if (string.IsNullOrWhiteSpace(patientDto.Email))
            {
                throw new BusinessRuleException("Patient email is required.");
            }
        }
    }
}