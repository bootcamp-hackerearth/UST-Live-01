using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Models.Dtos;

namespace HealthCareApp.Services
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<List<DoctorDto>> GetAllActiveDoctorsAsync();

        Task<DoctorDto> GetDoctorByIdAsync(int doctorId);

        Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(SpecialisationType specialisation);

        Task<List<DoctorDto>> GetActiveDoctorsBySpecialisationAsync(SpecialisationType specialisation);

        Task<DoctorCreatedResponseDto> CreateDoctorByAdminAsync(CreateDoctorDto dto);

        Task<DoctorDto> UpdateDoctorAsync(int doctorId, UpdateDoctorDto dto);

        Task<DoctorDto> DeleteDoctorAsync(int doctorId);
    }
}