using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

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

    Task<List<PatientDto>> GetPatientsAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null);
}