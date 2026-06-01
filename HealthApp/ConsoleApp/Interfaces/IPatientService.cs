using System;
using HealthApp.ConsoleApp.Models;
using System.Collections.Generic;

namespace HealthApp.ConsoleApp.Interfaces
{
    public interface IPatientService
    {
        string RegisterPatient(Patient patient);
        Patient UpdatePatient(Patient patient);
        Patient GetPatientById(int id); 
        List<Patient> GetAllPatients();
        List<Patient> GetPatientByName(string name);
    }
}
