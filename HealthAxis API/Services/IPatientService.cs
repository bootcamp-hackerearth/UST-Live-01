using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Models;

namespace HealthAxis.API.Services
{
    public interface IPatientService
        : IService<Patient, PatientReadDto, PatientCreateDto, PatientUpdateDto>
    {
    }
}
