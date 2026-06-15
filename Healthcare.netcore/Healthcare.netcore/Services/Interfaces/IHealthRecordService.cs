using HealthAxis.API.Dtos.HealthRecordDtos;

namespace HealthAxis.API.Services.Interfaces;

public interface IHealthRecordService
{
    Task<List<HealthRecordDto>> GetPatientRecordsAsync(int patientId);

    Task<HealthRecordDto> CreateHealthRecordAsync(CreateHealthRecordDto dto);
}