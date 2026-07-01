using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<List<DoctorDto>> GetDoctorsAsync(
            string? specialisation,
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<PagedResultDto<DoctorDto>> GetPagedAsync(
            string? specialisation,
            ClaimsPrincipal user,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<DoctorDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<DoctorDto> UpdateOwnStatusAsync(
            int id,
            bool isActive,
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<List<string>> GetAvailabilityAsync(
            int id,
            DateTime date,
            CancellationToken ct = default);
    }
}