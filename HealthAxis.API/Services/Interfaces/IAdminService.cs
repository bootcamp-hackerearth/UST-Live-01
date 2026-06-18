using HealthAxis.API.DTO;
using HealthAxis.DTO.DoctorDto;

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