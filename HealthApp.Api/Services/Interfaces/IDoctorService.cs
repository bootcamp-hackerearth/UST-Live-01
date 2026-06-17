using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IDoctorService
    {
            Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync();
            Task<DoctorDto?> GetDoctorByIdAsync(int id);
            Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto dto);
            Task<DoctorDto?> UpdateDoctorAsync(int id, DoctorCreateDto dto);

            Task<IEnumerable<DoctorDto>> SearchDoctorsAsync(string? search, SpecialisationType? specialization, bool? isActive);

            Task<bool> ChangeDoctorStatusAsync(int id, bool isActive);
    }
}
