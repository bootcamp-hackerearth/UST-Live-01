using HealthApp.Api.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IPatientService
    {
        Task<PatientDto> AddPatientAsync(PatientDto patientDto);

        Task<PatientDto> GetPatientByIdAsync(int id);

        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PatientDto> UpdatePatientByIdAsync(int id, PatientDto patientDto);
    }
}
