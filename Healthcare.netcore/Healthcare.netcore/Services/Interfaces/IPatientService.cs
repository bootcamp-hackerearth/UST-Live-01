using HealthAxis.API.DTOs;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);

        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);

        Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId);
    }
}
