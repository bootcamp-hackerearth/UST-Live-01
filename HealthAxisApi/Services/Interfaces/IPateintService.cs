using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDTO>> GetAllAsync();

        Task<PagedResponseDTO<PatientResponseDTO>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? gender
);

        Task<PatientResponseDTO?> GetByIdAsync(int id);

        Task<PatientResponseDTO> CreateAsync(CreatePatientDTO dto);

        Task<bool> UpdateAsync(int id, UpdatePatientDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<PatientResponseDTO>> SearchAsync(string? name, string? email);
    }
}