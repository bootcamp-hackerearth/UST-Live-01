using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class HealthRecordService
        : Service<HealthRecord, HealthRecordReadDto, HealthRecordCreateDto, HealthRecordUpdateDto>, IHealthRecordService
    {
        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IMapper mapper)
            : base(healthRecordRepository, mapper)
        {
        }
    }
}
