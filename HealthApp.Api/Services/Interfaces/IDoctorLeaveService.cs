using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IDoctorLeaveService
    {
        Task<DoctorLeavePreviewDto> PreviewLeaveAsync(
            int doctorId,
            DoctorLeaveCreateDto dto,
            CancellationToken ct = default);

        Task<DoctorLeaveCreationResultDto> CreateLeaveAsync(
            int doctorId,
            DoctorLeaveCreateDto dto,
            CancellationToken ct = default);

        Task<IEnumerable<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<DoctorLeaveDto?> GetLeaveForDateAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default);

        Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default);
    }
}