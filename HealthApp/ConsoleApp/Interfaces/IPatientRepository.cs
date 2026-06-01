using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{
    public interface IPatientRepository
    {
        string RegisterPatient(Patient patient);
        Patient UpdatePatient(Patient existingPatient, Patient patient);
        List<Patient> GetAllPatients();
        Patient? GetPatientById(int id);
        List<Patient> GetPatientByName(string name);
    }
}
