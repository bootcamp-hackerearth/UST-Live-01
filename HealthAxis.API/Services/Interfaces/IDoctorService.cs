using HealthAxis.DTO.DoctorDto;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto doctorDto);
        Task<DoctorDto?> UpdateAsync(int id, DoctorDto doctorDto);
    }
}