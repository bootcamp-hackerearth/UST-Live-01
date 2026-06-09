using System.Collections.Generic;
using HealthAxis.Shared.Dtos;

public interface IPatientService
{
    List<PatientDto> GetAllPatients();

    PatientDto GetById(int id);

    PatientDto AddPatient(PatientDto patientDto);

    PatientDto UpdatePatient(int id, PatientDto patientDto);
}