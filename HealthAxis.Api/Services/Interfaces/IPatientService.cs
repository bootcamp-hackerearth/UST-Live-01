using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync(
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<PatientDto> GetByIdAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<PatientDto> UpdatePatientAsync(
            int id,
            UpdatePatientDto request,
            ClaimsPrincipal user,
            CancellationToken ct = default);

        Task<List<HealthRecordDto>> GetHealthRecordsAsync(
            int id,
            ClaimsPrincipal user,
            CancellationToken ct = default);
    }
}