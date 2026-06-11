using HealthcareApi.Models;
using System.Collections.Generic;

namespace HealthcareApi.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();

        Patient GetById(int patientId);

        Patient Add(Patient patient);

        Patient Update(int patientId, Patient patient);

        Patient Delete(int patientId);

        List<Patient> SearchPatients(string query);
    }
}