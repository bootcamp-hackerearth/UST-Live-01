using HealthAxis.API.Dtos.PatientDtos;

namespace HealthAxis.API.Services.Interfaces;

public interface IPatientService
{
    Task<PatientDto?> GetPatientByIdAsync(int id);

    Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto dto);
}