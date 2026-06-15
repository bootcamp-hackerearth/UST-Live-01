using AutoMapper;
using HealthAxis.API.Dtos.HealthRecordDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Impl;

public class HealthRecordService : IHealthRecordService
{
    private readonly IHealthRecordRepository _healthRecordRepository;
    private readonly IMapper _mapper;

    public HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IMapper mapper)
    {
        _healthRecordRepository = healthRecordRepository;
        _mapper = mapper;
    }

    public async Task<List<HealthRecordDto>> GetPatientRecordsAsync(int patientId)
    {
        var records =
            await _healthRecordRepository.GetByPatientIdAsync(patientId);

        return _mapper.Map<List<HealthRecordDto>>(records);
    }

    public async Task<HealthRecordDto> CreateHealthRecordAsync(
        CreateHealthRecordDto dto)
    {
        var record = _mapper.Map<HealthRecord>(dto);

        await _healthRecordRepository.AddAsync(record);

        return _mapper.Map<HealthRecordDto>(record);
    }
}