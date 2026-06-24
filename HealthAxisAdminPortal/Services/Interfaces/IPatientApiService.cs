using HealthAxisApplicn.Dto.Patients;

namespace HealthAxisAdminPortal.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<List<PatientDto>?> GetAllAsync();
        Task<List<PatientDto>?> SearchAsync(string? name, string? phone);
        Task<bool> UpdateAsync(int id, UpdatePatientDto dto);
        Task<bool> ToggleAsync(int id);
    }
}
