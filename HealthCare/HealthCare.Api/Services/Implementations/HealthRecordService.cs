using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.HealthRecord;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;


namespace HealthCare.Api.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        private readonly IHealthRecordRepository _repository;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;

        public HealthRecordService(IHealthRecordRepository repository, IMapper mapper, HealthCareDbContext context)
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


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor")]

        public async Task UpdateAsync(int id,UpdateHealthRecordDto dto)
        {
            var record = await _repository.GetByIdAsync(id);
            if (record == null)
                throw new HealthRecordNotFoundException(id);

            _mapper.Map(dto,record);

            await _repository.UpdateAsync(record);
            await _context.SaveChangesAsync();
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

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

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<PagedResult<HealthRecordListDto>> GetAllAsync(HealthRecordFilter filter)
        {
            // Build predicate (date filtering)
            Expression<Func<HealthRecord, bool>>? predicate = null;

            if (filter.VisitDate.HasValue)
            {
                var start = filter.VisitDate.Value.ToDateTime(TimeOnly.MinValue); // 00:00
                var end = start.AddDays(1); // next day

                predicate = hr => hr.VisitDate >= start && hr.VisitDate < end;
            }

            // Ordering (by VisitDate)
            Func<IQueryable<HealthRecord>, IOrderedQueryable<HealthRecord>> orderBy =
                q => q.OrderBy(hr => hr.VisitDate);

            // Call repository
            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            // Map result
            return new PagedResult<HealthRecordListDto>
            {
                Items = _mapper.Map<IEnumerable<HealthRecordListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }


        [HttpGet("patient/{patientId:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        
        public async Task<List<HealthRecordListDto>> GetHealthRecordByPatient(int id)
        {
            var records = await _repository.GetHealthRecordByPatient(id);

            return _mapper.Map<List<HealthRecordListDto>>(records);
        }


        [HttpGet("appointment/{appointmentId:int}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<List<HealthRecordListDto>> GetHealthRecordByAppointment(int id)
        {
            var records = await _repository.GetHealthRecordByAppointment(id);

            return _mapper.Map<List<HealthRecordListDto>>(records);
        }


    }
}
