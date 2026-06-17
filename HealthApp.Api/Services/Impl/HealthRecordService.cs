using AutoMapper;
using HealthApp.Api.Dtos;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;

namespace HealthApp.Api.Services.Impl
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repo;
        private readonly IMapper _mapper;

        public HealthRecordService(IHealthRecordRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HealthRecordDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<HealthRecordDto>>(data);
        }

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return null;

            return _mapper.Map<HealthRecordDto>(data);
        }

        public async Task<HealthRecordDto> CreateAsync(HealthRecordCreateDto dto)
        {
            var entity = _mapper.Map<HealthRecord>(dto);

            var result = await _repo.Add(entity);
            return _mapper.Map<HealthRecordDto>(result);
        }
    }
}
