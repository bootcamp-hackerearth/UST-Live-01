using HealthApp.Api.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);
        Task<PatientDto> CreatePatientAsync(PatientCreateDto dto);
        Task<PatientDto?> UpdatePatientAsync(int id, PatientCreateDto dto);

        Task<IEnumerable<PatientDto>> SearchPatientsAsync(string? name, string? email);
    }
}
