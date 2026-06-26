using HealthApp.Shared.Dto;
using HealthApp.Api.Model;

namespace HealthApp.Api.Service.Interface
{
    public interface IPatientService
    {
        Task<PatientDto> AddPatientAsync(PatientDto patientDto);

        Task<PatientDto> GetPatientByIdAsync(int id);

        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PatientDto> UpdatePatientByIdAsync(int id, PatientDto patientDto);


        Task<PatientDto> GetMyProfileAsync(string identityUserId);
        Task<PatientDto> UpdateMyProfileAsync(string identityUserId, PatientDto patientDto);


    }
}
