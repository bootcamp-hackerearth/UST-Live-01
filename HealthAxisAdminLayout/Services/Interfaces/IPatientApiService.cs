using HealthAxisAdminLayout.DTOs.Patient;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<List<PatientResponseDTO>> GetPatientsAsync();

        Task<PatientResponseDTO?> GetPatientByIdAsync(int id);
    }
}