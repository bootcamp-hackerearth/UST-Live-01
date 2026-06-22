using S3_HealthAxis.Shared.DTOs.Patient;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();

        Task<PatientDto?> GetByIdAsync(int id);

        Task<IEnumerable<PatientSearchResultDto>> SearchByNameAsync(string name);

        Task<PatientDto> CreateAsync(CreatePatientDto dto);

        Task UpdateAsync(int id, UpdatePatientDto dto);

        Task DeactivateAsync(int id);

        Task ActivateAsync(int id);
    }
}