
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>> GetAllAsync();

        Task<HealthRecordDto> GetByIdAsync(int id);

        Task AddAsync(HealthRecordCreateDto dto);

        Task<IEnumerable<HealthRecordDto>> GetPatientHistoryAsync(int patientId);

        Task<IEnumerable<HealthRecordDto>> GetByPatientIdAsync(int patientId);

        Task<bool> ExistsByAppointmentIdAsync(int appointmentId);
    }
}