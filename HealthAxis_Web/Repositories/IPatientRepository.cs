using System.Collections.Generic;
using HealthAxis_Web.Models;

public interface IPatientRepository
{
    List<Patient> GetAllPatients();

    Patient GetById(int id);

    Patient AddPatient(Patient patient);

    Patient UpdatePatient(int id, Patient patient);
}