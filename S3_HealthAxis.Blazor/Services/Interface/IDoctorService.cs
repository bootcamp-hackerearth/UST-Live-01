using S3_HealthAxis.Shared.DTOs.Doctor;

namespace S3_HealthAxis.Blazor.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>?> GetAllAsync(string? sortBy = null, int? specialisation = null);
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorCreationResultDto?> CreateAsync(CreateDoctorDto dto);
        Task<bool> UpdateAsync(int id, UpdateDoctorDto dto);
        Task<bool> ActivateAsync(int id);
        Task<bool> DeactivateAsync(int id);
    }
}