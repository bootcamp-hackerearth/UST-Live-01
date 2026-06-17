using HealthAxis.API.DTO;
using HealthAxis.DTO.HealthRecordDto;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IHealthRecordService
    {
        //Task<List<HealthRecordDto>> GetAllAsync();

        //Task<HealthRecordDto?> GetByIdAsync(int id);

        //Task<HealthRecordDto> AddAsync(HealthRecordDto healthRecordDto);

        //Task<HealthRecordDto?> UpdateAsync(int id, HealthRecordDto healthRecordDto);

            Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId);

            Task<HealthRecordDto> GetByIdAsync(int id);

            Task<HealthRecordDto> AddAsync(CreateHealthRecordDto healthRecordDto);
        }
    }

