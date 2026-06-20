using HealthAxisCore_Api.Models.Dtos;
namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<DoctorDto>> GetDoctorsAsync(CancellationToken ct = default);
        Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto request, CancellationToken ct = default);
        Task<DoctorDto> UpdateDoctorAsync(int id, UpdateDoctorDto request, CancellationToken ct = default);
        Task<List<UserDto>> GetUsersAsync(string? role);
        Task<List<AppointmentReportDto>> GetAppointmentReportAsync(CancellationToken ct = default);
        Task UpdatePatientStatusAsync(int patientId, bool isActive, CancellationToken ct = default);
        Task UpdateDoctorStatusAsync(int doctorId, bool isActive, CancellationToken ct = default);
    }
}