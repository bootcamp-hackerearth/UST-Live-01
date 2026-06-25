using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        Task<PatientDto> GetPatientByIdAsync(
            int id);

        Task RegisterPatientAsync(
            PatientCreateDto dto);

        Task UpdatePatientAsync(
            int id,
            PatientCreateDto dto);

        Task<PagedResultDto<PatientDto>> SearchPatientsAsync(
            string? name,
            string? email,
            int pageNumber,
            int pageSize);
    }
}