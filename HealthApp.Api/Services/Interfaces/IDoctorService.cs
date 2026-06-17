using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();

        Task<DoctorDto> GetDoctorByIdAsync(int id);

        Task AddDoctorAsync(DoctorCreateDto dto);

        Task UpdateDoctorAsync(int id, DoctorCreateDto dto);

        Task ChangeStatusAsync(int id, bool isActive);

        Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialisationAsync(
            SpecialisationType specialisation);

        Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(
            string? search,
            SpecialisationType? specialisation,
            bool? isActive);
    }
}