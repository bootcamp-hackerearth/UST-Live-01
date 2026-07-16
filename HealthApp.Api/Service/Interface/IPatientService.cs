using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IPatientService
    {
        Task<PatientDto> AddPatientAsync(PatientDto patientDto);

        Task<(List<PatientDto> Items, int TotalCount)> GetPagedPatientsAsync(
            int pageNumber,
            int pageSize,
            string? search = null);

        Task<PatientDto> GetPatientByIdAsync(int id);

        Task<PatientDto> GetMyProfileAsync(string identityUserId);

        Task<PatientDto> UpdateMyProfileAsync(
            string identityUserId,
            PatientUpdateDto patientDto);
    }
}