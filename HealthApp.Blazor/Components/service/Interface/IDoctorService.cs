using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IDoctorService
    {

        Task<List<DoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDto?> GetDoctorByIdAsync(int id);
        Task<int> GetDoctorCountAsync();

        Task<bool> UpdateDoctorAsync(int id, DoctorDto dto);
        Task<List<DoctorDto>> SearchBySpecialisationAsync(string type);

        Task<bool> CreateDoctorAsync(DoctorDto dto);

    }
}
