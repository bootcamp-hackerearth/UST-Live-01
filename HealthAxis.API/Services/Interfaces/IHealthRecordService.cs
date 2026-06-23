using HealthAxis.Shared.DTO.HealthRecordDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId);

        Task<HealthRecordDto> GetByIdAsync(int id);

        Task<HealthRecordDto> AddAsync(CreateHealthRecordDto healthRecordDto, int loggedInDoctorId);
    }
}