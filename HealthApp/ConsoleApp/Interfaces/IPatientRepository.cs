using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{
    // Repository interface for managing patients
    public interface IPatientRepository
    {
        string RegisterPatient(Patient patient);
        Patient UpdatePatient(Patient existingPatient, Patient patient);
        Patient? GetPatientById(int id);
        List<Patient> GetAllPatients();
        List<Patient> GetPatientByName(string name);
    }
}
