using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken ct = default);
        Task<PatientDto?> GetPatientByIdAsync(int id, CancellationToken ct = default);
        Task<PatientDto> CreatePatientAsync(PatientCreateDto dto, CancellationToken ct = default);
        Task<PatientDto?> UpdatePatientAsync(int id, PatientCreateDto dto, CancellationToken ct = default);

        Task<IEnumerable<PatientDto>> SearchPatientsAsync(string? name, string? email, CancellationToken ct = default);
    }
}
