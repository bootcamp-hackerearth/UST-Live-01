using AutoMapper;
using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
namespace HealthApp.API.Service.Impl
{
    public class HealthRecordService(IHealthRecordRepository repository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto> AddAsync(HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            var savedEntity = await repository.AddAsync(healthRecord);
            return mapper.Map<HealthRecordDto>(savedEntity);
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetAllAsync());
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
