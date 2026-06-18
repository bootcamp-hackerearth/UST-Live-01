using AutoMapper;
using HealthAxisCore_Api.DTOs.HealthRecord;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using HealthAxisCore_Api.Exceptions;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ✅ Get All Records
        public async Task<IEnumerable<HealthRecordResponseDTO>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<HealthRecordResponseDTO>>(records);
        }

        // ✅ Get By Id
        public async Task<HealthRecordResponseDTO?> GetByIdAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record == null)
                throw new EntityNotFoundException("Health record not found");

            return _mapper.Map<HealthRecordResponseDTO>(record);
        }

        // ✅ Create Health Record
        public async Task<HealthRecordResponseDTO> CreateAsync(CreateHealthRecordDTO dto)
        {
            var record = _mapper.Map<HealthRecord>(dto);

            record.CreatedDate = DateTime.Now;

            await _repository.AddAsync(record);

            return _mapper.Map<HealthRecordResponseDTO>(record);
        }

        // ✅ Delete Record
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                throw new EntityNotFoundException("Health record not found");

            await _repository.DeleteAsync(id);

            return true;
        }

        // ✅ Get Records By Patient
        public async Task<IEnumerable<HealthRecordResponseDTO>> GetByPatientAsync(int patientId)
        {
            var records = await _repository.GetByPatient(patientId);

            // Optional: if you want strict handling
            if (records == null || !records.Any())
                throw new EntityNotFoundException("No health records found for this patient");

            return _mapper.Map<IEnumerable<HealthRecordResponseDTO>>(records);
        }
    }
}
