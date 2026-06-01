using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Exceptions;
using System;
using System.Collections.Generic;

namespace HealthAxis.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;

        public PatientService(IPatientRepository repository)
        {
            this._repository = repository;
        }

        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }

        public Patient? GetPatientById(int patientId)
        {
            var patient = _repository.GetPatientById(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException($"Patient with id {patientId} not registered.");
            }

            return patient;
        }

        public Patient RegisterPatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient is required.");

            var result = _repository.RegisterPatient(patient);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to register patient.");
            }
            return result;
        }

        public bool UpdatePatient(Patient patient)
        {
            if (patient == null)
                throw new ArgumentException("Patient is required.");

            if (string.IsNullOrWhiteSpace(patient.FullName))
                throw new ArgumentException("Patient name is required.");

            return _repository.UpdatePatient(patient);
        }
    }
}