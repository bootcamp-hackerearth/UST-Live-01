using S3_HealthAxis.Shared.DTOs.Patient;

namespace S3_HealthAxis.Blazor.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>?> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<List<PatientSearchResultDto>?> SearchByNameAsync(string name);
        Task<PatientDto?> CreateAsync(CreatePatientDto dto);
        Task<bool> UpdateAsync(int id, UpdatePatientDto dto);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}