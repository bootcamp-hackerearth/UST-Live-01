using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.Shared.DTO.AdminDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<List<AdminUserDto>> GetUsersAsync();

        Task<DoctorCreatedDto> AddDoctorAsync(CreateDoctorDto doctorDto);

        Task<DoctorDto> UpdateDoctorAsync(int id, UpdateDoctorDto doctorDto);

        Task<List<AdminDto>> GetAppointmentReportsAsync();
    }
}