using HealthAxis.API.DTO.AdminDtos;
using HealthAxis.API.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<DoctorCreatedDto> AddDoctorAsync(CreateDoctorDto doctorDto);

        Task<DoctorDto> UpdateDoctorAsync(int id, UpdateDoctorDto doctorDto);

        Task<List<AdminDto>> GetAppointmentReportsAsync();
    }
}