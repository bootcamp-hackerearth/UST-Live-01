using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class HealthRecordService
        : Service<HealthRecord, HealthRecordReadDto, HealthRecordCreateDto, HealthRecordUpdateDto>,
          IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IMapper mapper)
            : base(healthRecordRepository, mapper)
        {
            _healthRecordRepository = healthRecordRepository;
            _mapper = mapper;
        }

        public async Task<List<HealthRecordReadDto>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            List<HealthRecord> records =
                await _healthRecordRepository.GetByPatientIdAsync(patientId, ct);

            return _mapper.Map<List<HealthRecordReadDto>>(records);
        }
    }
}
