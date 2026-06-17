using HealthAxis.DTO.DoctorDto;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();

        Task<DoctorDto> GetByIdAsync(int id);

        Task<DoctorDto> GetAvailabilityAsync(int id);
    }
}