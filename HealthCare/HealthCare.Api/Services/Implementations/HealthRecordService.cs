using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs.HealthRecord;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;


namespace HealthCare.Api.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IRepository<HealthRecord> _repository;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;

        public HealthRecordService(IRepository<HealthRecord> repository, IMapper mapper, HealthCareDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddAsync(CreateHealthRecordDto dto)
        {
            var record=_mapper.Map<HealthRecord>(dto);
            await _repository.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id,UpdateHealthRecordDto dto)
        {
            var record = await _repository.GetByIdAsync(id);
            if (record == null)
                throw new HealthRecordNotFoundException(id);

            _mapper.Map(dto,record);

            await _repository.UpdateAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var record = await _repository.GetByIdAsync(id);
            if (record == null)
                throw new HealthRecordNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<HealthRecordListDto> GetByIdAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);
            return record == null ? null : _mapper.Map<HealthRecordListDto?>(record);
        }


        public async Task<IEnumerable<HealthRecordListDto>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<HealthRecordListDto>>(records);
        }

    }
}
