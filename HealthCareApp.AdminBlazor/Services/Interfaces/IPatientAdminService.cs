using HealthCareApp.AdminBlazor.Dtos.Patients;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IPatientAdminService
    {
        Task<List<PatientDto>> GetAllPatientsAsync();

        Task<PatientDto?> GetPatientByIdAsync(int patientId);

        Task<PatientDto> CreatePatientAsync(CreatePatientDto request);

        Task<PatientDto?> UpdatePatientAsync(int patientId, UpdatePatientDto request);

        Task<bool> DeletePatientAsync(int patientId);
    }
}