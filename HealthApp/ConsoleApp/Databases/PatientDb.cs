using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Databases;

public class PatientDb
{
    public List<Patient> Patients { get; set; } = new List<Patient>();
}
