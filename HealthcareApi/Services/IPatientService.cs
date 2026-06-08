using HealthcareApi.Dtos;
using System.Collections.Generic;

namespace HealthcareApi.Services
{
    public interface IPatientService
    {
        List<PatientDto> GetAllPatients();

        PatientDto GetPatientById(int patientId);

        PatientDto RegisterPatient(CreatePatientDto dto);

        PatientDto UpdatePatient(int patientId, UpdatePatientDto dto);

        PatientDto DeletePatient(int patientId);
    }
}