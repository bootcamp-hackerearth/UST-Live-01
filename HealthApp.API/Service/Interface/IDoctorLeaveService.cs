using HealthApp.Shared.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IDoctorLeaveService
{
    Task<DoctorLeaveImpactDto> PreviewMyLeaveImpactAsync(
        CreateDoctorLeaveDto dto,
        CancellationToken ct = default);

    Task<DoctorLeaveCreationResultDto> CreateMyLeaveAsync(
        CreateDoctorLeaveDto dto,
        CancellationToken ct = default);

    Task<List<DoctorLeaveDto>> GetMyLeaveHistoryAsync(
        CancellationToken ct = default);

    Task<List<DoctorLeaveDto>> GetDoctorLeaveHistoryAsync(
        int doctorId,
        CancellationToken ct = default);

    Task<DoctorLeaveStatusDto> GetDoctorLeaveStatusAsync(
        int doctorId,
        DateTime date,
        CancellationToken ct = default);
}