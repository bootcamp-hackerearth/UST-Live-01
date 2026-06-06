using System.Collections.Generic;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Services
{
    public interface IPatientService
    {
        Patient RegisterPatient(Patient patient);

        Patient GetPatientById(int patientId);

        List<Patient> GetAllPatients();

        Patient UpdatePatient(Patient patient);

        Patient DeletePatient(int patientId);
    }
}