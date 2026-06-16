using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Models;

namespace HealthAxisApplicn.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto?>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(DoctorDto entity);
        Task<DoctorDto?> UpdatebyAsync(int id, DoctorDto entity);
        Task<List<DoctorDto>> SearchDoctorByNameAsync(string name);
        Task<List<DoctorDto>> GetAvailableDoctorsAsync();
        Task<List<DoctorDto>> SearchBySpecialisationAsync(string specialisation);
    }
}
