using HealthAxisCore_Api.Models.Dtos;
using System.Security.Claims;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetByPatientIdAsync(int patientId, ClaimsPrincipal user, CancellationToken ct = default);

        Task<HealthRecordDto> GetByIdAsync(int id, ClaimsPrincipal user, CancellationToken ct = default);

        Task<HealthRecordDto> CreateAsync(CreateHealthRecordDto request, ClaimsPrincipal user, CancellationToken ct = default);
    }
}