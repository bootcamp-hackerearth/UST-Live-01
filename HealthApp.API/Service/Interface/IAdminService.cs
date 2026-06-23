using HealthApp.Shared.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IAdminService
{
    Task<List<DoctorDto>> GetDoctorsAsync();

    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto);

    Task<DoctorDto> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto dto);

    Task<List<AppointmentReportDto>> GetAppointmentReportsAsync();
    Task<List<UserDto>> GetUsersAsync(string? role = null);
}