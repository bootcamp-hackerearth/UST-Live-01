using System;
using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.ConsoleApp.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDb _patientsDb;

        public PatientRepository(PatientDb patientDb)
        {
            _patientsDb = patientDb;
        }

        public string RegisterPatient(Patient patient)
        {
            _patientsDb.Patients.Add(patient);
            return $"Patient ID {patient.PatientId} added successfully!";
        }

        public List<Patient> GetAllPatients()
        {
            return _patientsDb.Patients.ToList();
        }

        public List<Patient> GetPatientByName(string name)
        {
            return _patientsDb.Patients
                .Where(d => d.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Patient UpdatePatient(Patient existingPatient, Patient patient)
        {
            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;

            return existingPatient;
        }

        public Patient? GetPatientById(int id)
        {
            return _patientsDb.Patients.FirstOrDefault(p => p.PatientId == id);
        }
    }
}
