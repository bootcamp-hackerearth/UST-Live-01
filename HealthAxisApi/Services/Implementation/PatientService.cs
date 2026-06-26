using AutoMapper;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ✅ Get All Patients
        public async Task<IEnumerable<PatientResponseDTO>> GetAllAsync()
        {
            var patients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientResponseDTO>>(patients);
        }

        public async Task<PagedResponseDTO<PatientResponseDTO>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? gender)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var patients = await _repository.GetAllAsync();

            var query = patients.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.PatientName) &&
                     p.PatientName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    (!string.IsNullOrWhiteSpace(p.Email) &&
                     p.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    (!string.IsNullOrWhiteSpace(p.PhoneNumber) &&
                     p.PhoneNumber.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(gender) && gender != "All")
            {
                query = query.Where(p =>
                    p.Gender.ToString().Equals(
                        gender,
                        StringComparison.OrdinalIgnoreCase
                    ));
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedPatients = query
                .OrderBy(p => p.PatientName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var patientDtos = _mapper.Map<List<PatientResponseDTO>>(pagedPatients);

            return new PagedResponseDTO<PatientResponseDTO>
            {
                Items = patientDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        // ✅ Get Patient by Id
        public async Task<PatientResponseDTO?> GetByIdAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient == null)
                throw new EntityNotFoundException("Patient not found");

            return _mapper.Map<PatientResponseDTO>(patient);
        }

        // ✅ Create Patient
        public async Task<PatientResponseDTO> CreateAsync(CreatePatientDTO dto)
        {
            // ✅ Duplicate check
            var isDuplicate = await _repository.IsDuplicate(dto.Email, dto.PhoneNumber);

            if (isDuplicate)
                throw new BusinessRuleException("Patient already exists with same email or phone number");

            var patient = _mapper.Map<Patient>(dto);

            patient.CreatedDate = DateTime.Now;

            await _repository.AddAsync(patient);

            return _mapper.Map<PatientResponseDTO>(patient);
        }

        // ✅ Update Patient
        public async Task<bool> UpdateAsync(int id, UpdatePatientDTO dto)
        {
            var patient = await _repository.GetByIdAsync(id);

            if (patient == null)
                throw new EntityNotFoundException("Patient not found");

            _mapper.Map(dto, patient);

            await _repository.UpdateAsync(patient);

            return true;
        }

        // ✅ Delete Patient
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                throw new EntityNotFoundException("Patient not found");

            await _repository.DeleteAsync(id);

            return true;
        }

        // ✅ Search Patients
        public async Task<IEnumerable<PatientResponseDTO>> SearchAsync(string? name, string? email)
        {
            var patients = await _repository.SearchPatients(name, email);

            return _mapper.Map<IEnumerable<PatientResponseDTO>>(patients);
        }
    }
}