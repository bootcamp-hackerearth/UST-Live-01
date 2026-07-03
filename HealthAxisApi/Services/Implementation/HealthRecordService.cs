using AutoMapper;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;

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

        public async Task<IEnumerable<HealthRecordResponseDto>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<HealthRecordResponseDto>>(records);
        }

        public async Task<HealthRecordResponseDto?> GetByIdAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);

            if (record == null)
            {
                throw new EntityNotFoundException("Health record not found");
            }

            return _mapper.Map<HealthRecordResponseDto>(record);
        }

        public async Task<HealthRecordResponseDto> CreateAsync(CreateHealthRecordDto dto)
        {
            var alreadyExists = await _repository.ExistsByAppointmentIdAsync(dto.AppointmentId);

            if (alreadyExists)
            {
                throw new BusinessRuleException("Health record already exists for this appointment.");
            }

            var record = _mapper.Map<HealthRecord>(dto);

            record.CreatedDate = DateTime.Now;

            await _repository.AddAsync(record);

            return _mapper.Map<HealthRecordResponseDto>(record);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
            {
                throw new EntityNotFoundException("Health record not found");
            }

            await _repository.DeleteAsync(id);

            return true;
        }

        public async Task<IEnumerable<HealthRecordResponseDto>> GetByPatientAsync(int patientId)
        {
            var records = await _repository.GetByPatient(patientId);

            if (records == null || !records.Any())
            {
                throw new EntityNotFoundException("No health records found for this patient");
            }

            return _mapper.Map<IEnumerable<HealthRecordResponseDto>>(records);
        }
    }
}