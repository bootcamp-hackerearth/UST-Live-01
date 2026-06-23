
using HealthApp.Shared.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IHealthRecordService
{
    Task<List<HealthRecordDto>> GetAllHealthRecordsAsync();

    Task<HealthRecordDto> GetHealthRecordByIdAsync(int healthRecordId);

    Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId);

    Task<HealthRecordDto> AddHealthRecordAsync(AddHealthRecordDto dto);
}