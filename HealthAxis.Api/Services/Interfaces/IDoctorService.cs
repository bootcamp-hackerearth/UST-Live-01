using HealthAxisCore_Api.Models.DTOs;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorDto> UpdateAsync(int id,DoctorDto entity);
    }
}
