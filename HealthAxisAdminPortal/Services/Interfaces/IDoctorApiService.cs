using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Dto.Doctors;

namespace HealthAxisAdminPortal.Services.Interfaces
{

    public interface IDoctorApiService
    {
        Task<List<DoctorDto>?> GetAllAsync();

        Task<bool> CreateAsync(RegisterDto register, CreateDoctorDto doctor);

        Task<bool> UpdateAsync(int id, UpdateDoctorDto dto);

        Task<bool> ToggleActiveAsync(int id);
        Task<List<DoctorDto>?> SearchAsync(string query);
    }

}
