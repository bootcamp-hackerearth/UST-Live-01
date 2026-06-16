using AutoMapper;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.HealthRecordDto;

namespace HealthAxis.API.Services.Implementation
{
    public class HealthRecordService(IHealthRecordRepository repository,IMapper mapper) : IHealthRecordService
    {
        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetAllAsync());
        }

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            return mapper.Map<HealthRecordDto>(await repository.GetByIdAsync(id));
        }

        public async Task<HealthRecordDto> AddAsync(HealthRecordDto healthRecordDto)
        {
            var healthRecord = mapper.Map<HealthRecord>(healthRecordDto);

            var saved = await repository.AddAsync(healthRecord);

            return mapper.Map<HealthRecordDto>(saved);
        }

        public async Task<HealthRecordDto?> UpdateAsync(int id, HealthRecordDto healthRecordDto)
        {
            var healthRecord = mapper.Map<HealthRecord>(healthRecordDto);

            healthRecord.RecordId = id;

            var updated =await repository.UpdateAsync(id, healthRecord);

            return mapper.Map<HealthRecordDto>(updated);
        }

        public async Task<HealthRecordDto?> DeleteAsync(int id)
        {
            var deleted = await repository.DeleteAsync(id);

            return mapper.Map<HealthRecordDto>(deleted);
        }
    }
}