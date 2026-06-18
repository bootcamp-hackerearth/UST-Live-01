using HealthAxis.API.DTOs;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync(
            CancellationToken ct = default);

        Task<DoctorDto?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<object> GetAvailabilityAsync(int id);

        // ✅ Used by AdminController
        Task<DoctorDto> AddAsync(CreateDoctorDto dto);

        // ✅ Used by AdminController
        Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto);
    }
}