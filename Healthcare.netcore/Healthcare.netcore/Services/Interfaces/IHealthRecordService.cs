using HealthAxis.API.DTOs;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>> GetByPatientIdAsync(int patientId);

        Task<IEnumerable<HealthRecordDto>> GetByDoctorIdAsync(int doctorId);

        Task<HealthRecordDto> GetByIdAsync(int id);

        Task<HealthRecordDto> AddAsync(CreateHealthRecordDto dto);
    }
}