using System;
using System.Collections.Generic;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Repositories;

namespace HealthcareMvcApp.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Patient RegisterPatient(Patient patient)
        {
            ValidatePatient(patient);

            patient.CreatedDate = DateTime.Today;

            _patientRepository.Add(patient);

            return patient;
        }

        public Patient GetPatientById(int patientId)
        {
            ValidatePatientId(patientId);

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return patient;
        }

        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAll();
        }

        public Patient UpdatePatient(Patient patient)
        {
            ValidatePatient(patient);
            ValidatePatientId(patient.PatientId);

            Patient existingPatient = _patientRepository.GetById(patient.PatientId);

            if (existingPatient == null)
            {
                throw new EntityNotFoundException("Patient", patient.PatientId);
            }

            bool updated = _patientRepository.Update(patient);

            if (!updated)
            {
                throw new EntityNotFoundException("Patient", patient.PatientId);
            }

            return patient;
        }

        private static void ValidatePatient(Patient patient)
        {
            if (patient == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            if (string.IsNullOrWhiteSpace(patient.FullName))
            {
                throw new BusinessRuleException("Patient full name is required.");
            }

            if (patient.DateOfBirth.Date < new DateTime(1900, 1, 1))
            {
                throw new BusinessRuleException(
                    "Date of birth cannot be before 01 Jan 1900.");
            }

            if (patient.DateOfBirth.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Date of birth cannot be in the future.");
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new BusinessRuleException("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new BusinessRuleException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                throw new BusinessRuleException("Insurance ID is required.");
            }
        }

        private static void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Valid Patient ID is required.");
            }
        }

        public Patient DeletePatient(int patientId)
        {
            ValidatePatientId(patientId);

            Patient existingPatient = _patientRepository.GetById(patientId);

            if (existingPatient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            bool deleted = _patientRepository.Delete(patientId);

            if (!deleted)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return existingPatient;
        }
    }
}