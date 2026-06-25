using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;

namespace HealthApp.API.Service.Interface;

public interface IAdminService
{
    Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
            PaginationQueryDto? pagination = null);

    Task<CreateDoctorResponseDto> CreateDoctorAsync(CreateDoctorDto dto);

    Task<DoctorDto> UpdateDoctorAsync(
        int doctorId,
        UpdateDoctorDto dto);

    Task<PagedResultDto<AppointmentReportDto>> GetAppointmentReportsAsync(
        PaginationQueryDto? pagination = null);
   
    Task<List<UserDto>> GetUsersAsync(string? role = null);

    Task<PagedResultDto<PatientDto>> GetPatientsAsync(
        string? search = null,
        GenderType? gender = null,
        bool? hasInsurance = null,
        PaginationQueryDto? pagination = null);
}