using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IPatientService
    {
        Task<PagedResponse<PatientDto>> GetPagedPatientsAsync(int pageNumber, int pageSize,
            string? search = null);

        Task<PatientDto?> GetPatientByIdAsync(int id);

        Task<int> GetPatientCountAsync();
    }
}