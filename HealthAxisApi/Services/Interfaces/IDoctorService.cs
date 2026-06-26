using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDTO>> GetAllAsync();

        Task<DoctorResponseDTO?> GetByIdAsync(int id);

        Task<DoctorResponseDTO> CreateAsync(CreateDoctorDTO dto);

        Task<bool> UpdateAsync(int id, CreateDoctorDTO dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<DoctorResponseDTO>> FilterAsync(string? name, SpecialisationType? specialization, bool? isActive);

        Task<bool> SetStatusAsync(int doctorId, bool status);

        Task<PagedResponseDTO<DoctorResponseDTO>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? specialisation,
    string? status
);
    }
}
