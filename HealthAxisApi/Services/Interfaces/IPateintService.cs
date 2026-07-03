using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();

        Task<PagedResponseDto<PatientResponseDto>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? gender
);

        Task<PatientResponseDto?> GetByIdAsync(int id);

        Task<PatientResponseDto> CreateAsync(CreatePatientDto dto);

        Task<bool> UpdateAsync(int id, UpdatePatientDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<PatientResponseDto>> SearchAsync(string? name, string? email);
    }
}