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

            if (!string.IsNullOrWhiteSpace(patient.InsuranceID))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(
                        patient.InsuranceID,
                        "^INS\\d{4}$",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase,
                        TimeSpan.FromMilliseconds(100)))
                {
                    throw new ArgumentException("Insurance ID must follow format INSXXXX where X are digits.");
                }

                patient.InsuranceID = patient.InsuranceID.ToUpperInvariant();
            }

            var result = _repository.RegisterPatient(patient);

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