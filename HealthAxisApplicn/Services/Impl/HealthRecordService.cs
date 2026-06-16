using AutoMapper;
using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class HealthRecordService(IHealthRecordRepository repository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto> CreateAsync(HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            var savedEntity = await repository.CreateAsync(healthRecord);
            return mapper.Map<HealthRecordDto>(savedEntity);

        }

        public async Task<List<HealthRecordDto?>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto?>>(await repository.GetAllAsync());
        }

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            var healthRecord = await repository.GetByIdAsync(id);
            return mapper.Map<HealthRecordDto?>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetRecordByPatientIDAsync(int patientId)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordByPatientIDAsync(patientId));
        }

        public async Task<List<HealthRecordDto>> GetRecordsByDoctorIDAsync(int doctorId)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordsByDoctorIDAsync(doctorId));
        }

        public async Task<HealthRecordDto?> UpdatebyAsync(int id, HealthRecordDto entity)
        {
            var healthRecord = mapper.Map<HealthRecord>(entity);
            var updatedHealthRecord = await repository.UpdatebyAsync(id, healthRecord);
            return mapper.Map<HealthRecordDto?>(updatedHealthRecord);
        }
    }
}
