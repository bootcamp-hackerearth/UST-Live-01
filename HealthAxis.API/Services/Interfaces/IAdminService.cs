using HealthAxis.Shared.DTO;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IAdminService
    {
        Task<DoctorCreatedDto> AddDoctorAsync(CreateDoctorDto doctorDto);

        Task<List<DoctorDto>> GetAllDoctorsAsync();

        Task<PagedResponseDto<DoctorDto>> GetDoctorsPagedAsync(
            PaginationQueryDto paginationQuery);

        Task<DoctorDto> UpdateDoctorAsync(
            int id,
            UpdateDoctorDto doctorDto);

        Task<List<AdminDto>> GetAppointmentReportsAsync();

        Task<List<AdminUserDto>> GetUsersAsync();

        Task<PagedResponseDto<AdminUserDto>> GetUsersPagedAsync(
            AdminUserQueryDto queryDto);
        Task<List<AdminAppointmentDetailDto>> GetAppointmentDetailsAsync();

        Task<AdminProfileDto> GetAdminProfileAsync(string userId);

        Task<AdminProfileDto> UpdateAdminProfileAsync(
            string userId,
            UpdateAdminProfileDto profileDto);

        Task ChangeAdminPasswordAsync(
            string userId,
            ChangePasswordDto passwordDto);

        Task<List<AdminPatientDto>> GetPatientsAsync();

        Task<AdminPatientDto> UpdatePatientAsync(
            int patientId,
            UpdateAdminPatientDto patientDto);

        Task<List<AdminPatientAppointmentDto>> GetPatientAppointmentsAsync(
            int patientId);
    }
}