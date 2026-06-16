using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto> GetByIdAsync(int id);
        Task<DoctorDto> AddAsync(DoctorDto entity);
        Task<DoctorDto> UpdateAsync(int id, DoctorDto entity);
    }
}
