using AutoMapper;
using HealthAxisCore_Api.DTOs.Patient;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;

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