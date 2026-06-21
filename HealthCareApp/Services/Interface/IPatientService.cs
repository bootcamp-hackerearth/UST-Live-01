using HealthCareApp.Dtos;

namespace HealthCareApp.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PagedResponse<PatientDto>> GetAllPatientsPagedAsync(PatientPaginationQueryDto query);


        Task<PatientDto> GetPatientByIdAsync(int patientId);

        Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto);

        Task<PatientDto> UpdatePatientAsync(int patientId, UpdatePatientDto dto);

        Task<PatientDto> GetMyProfileAsync(string identityUserId);

        Task<PatientDto> UpdateMyProfileAsync(string identityUserId, UpdatePatientDto dto);


    }
}