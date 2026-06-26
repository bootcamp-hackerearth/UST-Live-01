using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<List<PatientResponseDTO>> GetPatientsAsync();

        Task<PatientResponseDTO?> GetPatientByIdAsync(int id);

        Task<List<PatientResponseDTO>> SearchPatientsAsync(string? name, string? email);

        Task<PagedResponseDTO<PatientResponseDTO>> GetPatientsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? gender
        );

        Task<bool> CreatePatientAsync(CreatePatientDTO dto);

        Task<bool> UpdatePatientAsync(int id, UpdatePatientDTO dto);

        Task<bool> DeletePatientAsync(int id);
    }
}