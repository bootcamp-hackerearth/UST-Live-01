
using HealthApp.Shared.Dto;

namespace HealthApp.Blazor.Components.service.Interface
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();
        Task<PatientDto?> GetPatientByIdAsync(int id);

        Task<int> GetPatientCountAsync();
    }
}
