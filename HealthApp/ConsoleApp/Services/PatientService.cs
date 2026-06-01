using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using HealthApp.ConsoleApp.Exceptions;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Repositories;


namespace HealthApp.ConsoleApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepo;

        public PatientService(IPatientRepository patientRepo)
        {
            _patientRepo = patientRepo;
        }

        public string RegisterPatient(Patient patient)
        {
            List<Patient> patients = _patientRepo.GetAllPatients();

            patient.PatientId = PatientIdGenerator(patients);

            return _patientRepo.RegisterPatient(patient);
        }

        public Patient UpdatePatient(Patient patient)
        {
            Patient? existingPatient = GetPatientById(patient.PatientId);

            if (existingPatient is null)
            {
                throw new PatientNotFoundException($"Patient of ID {patient.PatientId} does not exist");
            }
            return _patientRepo.UpdatePatient(existingPatient, patient);
        }

        public Patient GetPatientById(int id)
        {
            Patient? patient = _patientRepo.GetPatientById(id);

            if (patient is null)
            {
                throw new PatientNotFoundException($"Patient of ID {id} does not exist");
            }
            return patient;
        }

        public static int PatientIdGenerator(List<Patient> patients)
        {
            return patients.Count > 0
                ? patients.Max(p => p.PatientId) + 1
                : 101;
        }

        public List<Patient> GetAllPatients()
        {
            var patients = _patientRepo.GetAllPatients();

            if (patients == null || patients.Count == 0)
            {
                throw new PatientNotFoundException("No patients found.");
            }

            return patients;
        }

        public List<Patient> GetPatientByName(string name)
        {
            var result = _patientRepo.GetPatientByName(name);

            if (result == null || result.Count == 0)
            {
                throw new PatientNotFoundException($"Patient with name {name} does not exist");
            }

            return result;
        }
    }
}