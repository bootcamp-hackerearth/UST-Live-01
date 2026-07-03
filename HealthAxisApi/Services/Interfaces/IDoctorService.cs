using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDto>> GetAllAsync();

        Task<DoctorResponseDto?> GetByIdAsync(int id);

        Task<CreateDoctorResultDto> CreateAsync(CreateDoctorDto dto);

        Task<bool> UpdateAsync(int id, CreateDoctorDto dto);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<DoctorResponseDto>> FilterAsync(string? name, SpecialisationType? specialization, bool? isActive);

        Task<bool> SetStatusAsync(int doctorId, bool status);

        Task<PagedResponseDto<DoctorResponseDto>> GetPagedAsync(
    int pageNumber,
    int pageSize,
    string? search,
    string? specialisation,
    string? status
);
    }
}
