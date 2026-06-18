using HealthAxis.API.DTO.AdminDtos;
using HealthAxis.API.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<DoctorDto> AddDoctorAsync(DoctorDto doctorDto);

        Task<DoctorDto> UpdateDoctorAsync(int id, DoctorDto doctorDto);

        Task<List<AdminDto>> GetAppointmentReportsAsync();
    }
}