using AutoMapper;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Patients;

namespace HealthCareApp.Services
{
    public class PatientService(IPatientRepository repository, IMapper mapper) : IPatientService
    {
        private const string PatientEntityName = "Patient";

        private static readonly DateTime MinimumDateOfBirth = new(
            1900,
            1,
            1,
            0,
            0,
            0,
            DateTimeKind.Unspecified);

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await repository.GetAllAsync();

            return mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PagedResponse<PatientDto>> GetAllPatientsPagedAsync(PatientPaginationQueryDto query)
        {
            query ??= new PatientPaginationQueryDto();

            int pageNumber = query.PageNumber <= 0 ? 1 : query.PageNumber;

            int pageSize = query.PageSize <= 0 ? 10 : query.PageSize;

            pageSize = pageSize > 100 ? 100 : pageSize;

            var patients = await repository.GetAllAsync();

            var filteredPatients = patients.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                string searchTerm = query.SearchTerm.Trim();

                filteredPatients = filteredPatients.Where(patient =>
                    patient.PatientName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    patient.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    patient.PhoneNumber.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrWhiteSpace(patient.InsuranceID) &&
                     patient.InsuranceID.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            if (query.Gender is not null)
            {
                filteredPatients = filteredPatients.Where(patient =>
                    patient.Gender == query.Gender.Value);
            }

            if (query.HasInsurance is not null)
            {
                if (query.HasInsurance.Value)
                {
                    filteredPatients = filteredPatients.Where(patient =>
                        !string.IsNullOrWhiteSpace(patient.InsuranceID));
                }
                else
                {
                    filteredPatients = filteredPatients.Where(patient =>
                        string.IsNullOrWhiteSpace(patient.InsuranceID));
                }
            }

            int totalRecords = filteredPatients.Count();

            var pagedPatients = filteredPatients
                .OrderBy(patient => patient.PatientId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var mappedPatients = mapper.Map<List<PatientDto>>(pagedPatients);

            return new PagedResponse<PatientDto>
            {
                Items = mappedPatients,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };
        }

        public async Task<PatientDto> GetPatientByIdAsync(int patientId)
        {
            ValidatePatientId(patientId);

            var patient = await repository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new EntityNotFoundException(PatientEntityName, patientId);
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto)
        {
            ValidateCreatePatientDto(dto);

            string patientName = dto.FullName.Trim().ToLower();
            string email = dto.Email.Trim().ToLower();
            string phoneNumber = dto.PhoneNumber.Trim();
            string insuranceId = dto.InsuranceId?.Trim() ?? string.Empty;
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

            patient.PatientName = patientName;
            patient.Email = email;
            patient.PhoneNumber = phoneNumber;
            patient.InsuranceID = insuranceId;
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
                throw new EntityNotFoundException(PatientEntityName, patientId);
            }

            string patientName = dto.FullName.Trim().ToLower();
            string email = dto.Email.Trim().ToLower();
            string phoneNumber = dto.PhoneNumber.Trim();
            string insuranceId = dto.InsuranceId?.Trim() ?? string.Empty;
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
            patient.PatientName = patientName;
            patient.Email = email;
            patient.PhoneNumber = phoneNumber;
            patient.InsuranceID = insuranceId;
            patient.DateOfBirth = dateOfBirth;
            patient.CreatedDate = existingPatient.CreatedDate;
            patient.IdentityUserId = existingPatient.IdentityUserId;

            var updatedPatient = await repository.UpdateAsync(patientId, patient);

            if (updatedPatient is null)
            {
                throw new EntityNotFoundException(PatientEntityName, patientId);
            }

            return mapper.Map<PatientDto>(updatedPatient);
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

            ValidatePatientCommonFields(
                dto.FullName,
                dto.DateOfBirth,
                dto.Email,
                dto.PhoneNumber);

            var existingPatient = await repository.GetByIdentityUserIdAsync(identityUserId);

            if (existingPatient is null)
            {
                throw new EntityNotFoundException("Patient profile for logged-in user", 0);
            }

            string patientName = dto.FullName.Trim().ToLower();
            string phoneNumber = dto.PhoneNumber.Trim();
            string insuranceId = dto.InsuranceId?.Trim() ?? string.Empty;
            DateTime dateOfBirth = dto.DateOfBirth.Date;

            var patient = mapper.Map<Patient>(dto);

            patient.PatientId = existingPatient.PatientId;
            patient.PatientName = patientName;
            patient.IdentityUserId = existingPatient.IdentityUserId;
            patient.Email = existingPatient.Email.Trim().ToLower();
            patient.PhoneNumber = phoneNumber;
            patient.InsuranceID = insuranceId;
            patient.DateOfBirth = dateOfBirth;
            patient.CreatedDate = existingPatient.CreatedDate;

            var updatedPatient = await repository.UpdateAsync(existingPatient.PatientId, patient);

            if (updatedPatient is null)
            {
                throw new EntityNotFoundException(PatientEntityName, existingPatient.PatientId);
            }

            return mapper.Map<PatientDto>(updatedPatient);
        }

        private static void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid patient reference.");
            }
        }

        private static void ValidateCreatePatientDto(CreatePatientDto dto)
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

        private static void ValidateUpdatePatientDto(UpdatePatientDto dto)
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

        private static void ValidatePatientCommonFields(
            string patientName,
            DateTime dateOfBirth,
            string email,
            string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(patientName))
            {
                throw new BusinessRuleException("Patient name is required.");
            }

            if (dateOfBirth.Date < MinimumDateOfBirth)
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
    }
}