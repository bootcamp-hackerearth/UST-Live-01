using HealthAxis.API.Dtos.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces;

public interface IDoctorService
{
    Task<List<DoctorDto>> GetAllDoctorsAsync();

    Task<DoctorDto?> GetDoctorByIdAsync(int id);

    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto);

    Task<DoctorDto?> UpdateDoctorAsync(int id, UpdateDoctorDto dto);

    Task<bool> DeleteDoctorAsync(int id);
}