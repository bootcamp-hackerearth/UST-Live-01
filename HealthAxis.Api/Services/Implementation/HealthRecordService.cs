using AutoMapper;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.DTOs;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Interfaces;

namespace HealthAxisCore_Api.Services.Implementation
{
    public class HealthRecordService(IHealthRecordRepository repository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto> AddAsync(HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            var savedEntity = await repository.CreateAsync(healthRecord);
            return mapper.Map<HealthRecordDto>(savedEntity);
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto>>(await  repository.GetAllAsync());
        }

        public async Task<HealthRecordDto> GetByIdAsync(int id)
        {
            return mapper.Map<HealthRecordDto>(await repository.GetByIdAsync(id));
        }

        public async Task<HealthRecordDto> UpdateAsync(int id, HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            healthRecord.HealthRecordId = id;
            var updated = await repository.UpdateAsync(id, healthRecord);
            return mapper.Map<HealthRecordDto>(updated);
        }
    }
}
