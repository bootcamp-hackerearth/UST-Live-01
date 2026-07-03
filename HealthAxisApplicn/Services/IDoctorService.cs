using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(CreateDoctorDto entity);
        Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto entity);
        Task<List<DoctorDto>> SearchByNameAsync(string name);
        Task<List<DoctorDto>> GetActiveDoctorsAsync();
        Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation);
        Task<DoctorDto> DeactivateDoctorAsync(int id);
        Task<bool> ToggleActiveAsync(int id);
        Task<List<DoctorDto>> SearchAsync(string query);
        Task<Doctor?> GetByUserIdAsync(string userId);
        Task<List<DoctorDto>> FilterAsync(string? name, string? specialization);
    }
}
