
using HealthApp.Shared.DTOs;

namespace HealthApp.API.Service.Interface;

public interface IPatientService
{
    Task<List<PatientDto>> GetAllPatientsAsync();

    Task<PatientDto> GetPatientByIdAsync(int patientId);

    Task<PatientDto> RegisterPatientAsync(CreatePatientDto dto);

    Task<PatientDto> UpdatePatientAsync(
        int patientId,
        UpdatePatientDto dto);
}