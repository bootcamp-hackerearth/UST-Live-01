using HealthApp.Shared.Dto;

namespace HealthApp.Admin.Services.Interface
{
    public interface IPatientService
    {
        Task<PagedResponse<PatientDto>> GetPagedPatientsAsync(int pageNumber, int pageSize,
            string? search = null);

        Task<PatientDto?> GetPatientByIdAsync(int id);

        Task<int> GetPatientCountAsync();
    }
}