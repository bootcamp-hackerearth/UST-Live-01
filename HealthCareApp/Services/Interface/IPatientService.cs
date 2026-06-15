using HealthCareApp.Dtos;

namespace HealthCareApp.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PatientDto> GetPatientByIdAsync(int patientId);

        Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto);

        Task<PatientDto> UpdatePatientAsync(int patientId, UpdatePatientDto dto);

        Task<PatientDto> DeletePatientAsync(int patientId);
    }
}