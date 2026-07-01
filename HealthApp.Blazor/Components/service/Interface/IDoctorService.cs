using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IDoctorService
    {
        Task<DoctorDto?> GetDoctorByIdAsync(int id);

        Task<int> GetDoctorCountAsync();

        Task<(bool Success, string Message)> CreateDoctorAsync(DoctorDto dto);

        Task<(bool Success, string Message)> UpdateDoctorAsync(int id, DoctorDto dto);

        Task<PagedResponse<DoctorDto>> GetAllDoctorsAsync(int pageNumber, int pageSize);

        Task<PagedResponse<DoctorDto>> GetActiveDoctorsAsync(int pageNumber, int pageSize);

        Task<PagedResponse<DoctorDto>> SearchBySpecialisationAsync
            (string type, int pageNumber,int pageSize);
    }
}