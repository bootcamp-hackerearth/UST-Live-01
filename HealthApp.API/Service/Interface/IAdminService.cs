using HealthApp.API.Models.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IAdminService
{
    Task<List<DoctorDto>> GetDoctorsAsync();

    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto);

    Task<DoctorDto> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto dto);

    Task<List<AppointmentReportDto>> GetAppointmentReportsAsync();
}