using HealthAxis.Shared.DTO.HealthRecordDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>>
            GetByPatientIdAsync(int patientId);

        Task<List<HealthRecordDto>>
            GetByDoctorIdAsync(int doctorId);

        Task<HealthRecordDto?>
            GetByIdAsync(int id);

        Task<HealthRecordDto> AddAsync(
            CreateHealthRecordDto healthRecordDto,
            int loggedInDoctorId);

        Task<HealthRecordDto?> UpdateAsync(
            int id,
            UpdateHealthRecordDto healthRecordDto,
            int loggedInDoctorId);
    }
}