using System;
using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.ConsoleApp.Repositories
{
    // Repository class to manage patients in the healthcare system
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDb _patientsDb;

        public PatientRepository(PatientDb patientDb)
        {
            _patientsDb = patientDb;
        }

        // Method to register a new patient in the database
        public string RegisterPatient(Patient patient)
        {
            _patientsDb.Patients.Add(patient);
            return $"Patient ID {patient.PatientId} added successfully!";
        }
        // Method to get all patients from the database
        public List<Patient> GetAllPatients()
        {
            return _patientsDb.Patients.ToList();
        }
        // Method to update an existing patient in the database
        public Patient UpdatePatient(Patient existingPatient, Patient patient)
        {
            existingPatient.Name = patient.Name;
            existingPatient.Dob = patient.Dob;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;

            return existingPatient;
        }
        // Method to get a patient by ID from the database
        public Patient? GetPatientById(int id)
        {
            return _patientsDb.Patients.FirstOrDefault(p => p.PatientId == id);
        }
        public List<Patient> GetPatientByName(string name)
        {
            return _patientsDb.Patients
                .Where(d => d.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}