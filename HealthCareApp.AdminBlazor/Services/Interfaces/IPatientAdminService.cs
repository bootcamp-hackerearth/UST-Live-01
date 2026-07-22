using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Patients;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IPatientAdminService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PagedResponse<PatientDto>> GetPatientsPagedAsync(PatientPaginationQueryDto query);

        Task<PatientDto?> GetPatientByIdAsync(int patientId);

        Task<PatientDto?> CreatePatientAsync(CreatePatientDto patientDto);

        Task<PatientDto?> UpdatePatientAsync(int patientId, UpdatePatientDto patientDto);
    }
}
