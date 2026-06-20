using HealthAxis.API.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();

        Task<DoctorDto> GetByIdAsync(int id);

        Task<DoctorDto?> GetByUserIdAsync(string userId);

        Task<DoctorDto> GetAvailabilityAsync(int id);
    }
}