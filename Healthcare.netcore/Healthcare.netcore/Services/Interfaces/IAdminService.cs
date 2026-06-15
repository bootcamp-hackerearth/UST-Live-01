using HealthAxis.API.Dtos.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces;

public interface IAdminService
{
    Task<List<DoctorDto>> GetDoctorsAsync();

    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto);

    Task<DoctorDto?> UpdateDoctorAsync(
        int id,
        UpdateDoctorDto dto);
}