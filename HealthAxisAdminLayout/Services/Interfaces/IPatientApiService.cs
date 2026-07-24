using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;

namespace HealthAxisAdminLayout.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<List<PatientResponseDto>> GetPatientsAsync();

        Task<PatientResponseDto?> GetPatientByIdAsync(int id);

        Task<List<PatientResponseDto>> SearchPatientsAsync(string? name, string? email);

        Task<PagedResponseDto<PatientResponseDto>> GetPatientsPagedAsync(
            int pageNumber,
            int pageSize,
            string? search,
            string? gender
        );

        Task<bool> CreatePatientAsync(CreatePatientDto dto);

        Task<bool> UpdatePatientAsync(int id, UpdatePatientDto dto);

        Task<bool> DeletePatientAsync(int id);
    }
}

