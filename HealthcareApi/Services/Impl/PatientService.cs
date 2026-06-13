using SharedClasses.Dtos;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using System;
using System.Collections.Generic;

namespace HealthcareApi.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public List<PatientDto> GetAllPatients()
        {
            List<Patient> patients = _patientRepository.GetAll();
            return MapToDtoList(patients);
        }

        public PatientDto GetPatientById(int patientId)
        {
            ValidatePatientId(patientId);

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return MapToDto(patient);
        }

        public PatientDto RegisterPatient(CreatePatientDto dto)
        {
            // ✅ 1. Null check
            if (dto == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            // ✅ 2. Normalize input
            string name = dto.FullName.Trim().ToLower();
            string phone = dto.PhoneNumber.Trim();
            string email = dto.Email.Trim().ToLower();
            DateTime dob = dto.DateOfBirth.Date;

            // ✅ 3. Duplicate validation
            bool duplicate = _patientRepository.IsDuplicatePatient(name, phone, email, dob);

            if (duplicate)
            {
                throw new BusinessRuleException(
                    "A patient with similar details already exists.");
            }

            // ✅ 4. Create entity
            Patient patient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dob,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceId = dto.InsuranceId,
                CreatedDate = DateTime.Today
            };

            ValidatePatient(patient);

            Patient savedPatient = _patientRepository.Add(patient);

            return MapToDto(savedPatient);
        }

        public PatientDto UpdatePatient(int patientId, UpdatePatientDto dto)
        {
            ValidatePatientId(patientId);

            if (dto == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            Patient existingPatient = _patientRepository.GetById(patientId);

            if (existingPatient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            string name = dto.FullName.Trim().ToLower();
            string phone = dto.PhoneNumber.Trim();
            string email = dto.Email.Trim().ToLower();
            DateTime dob = dto.DateOfBirth.Date;

            bool duplicate = _patientRepository.IsDuplicatePatient(name, phone, email, dob);

            // ✅ Avoid conflict with itself
            if (duplicate &&
                !(existingPatient.FullName.ToLower() == name &&
                  existingPatient.PhoneNumber == phone &&
                  existingPatient.Email.ToLower() == email &&
                  existingPatient.DateOfBirth == dob))
            {
                throw new BusinessRuleException(
                    "Another patient with similar details already exists.");
            }

            existingPatient.FullName = dto.FullName;
            existingPatient.DateOfBirth = dob;
            existingPatient.Gender = dto.Gender;
            existingPatient.PhoneNumber = dto.PhoneNumber;
            existingPatient.Email = dto.Email;
            existingPatient.InsuranceId = dto.InsuranceId;

            ValidatePatient(existingPatient);

            Patient updatedPatient = _patientRepository.Update(patientId, existingPatient);

            if (updatedPatient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return MapToDto(updatedPatient);
        }

        public PatientDto DeletePatient(int patientId)
        {
            ValidatePatientId(patientId);

            Patient deletedPatient = _patientRepository.Delete(patientId);

            if (deletedPatient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return MapToDto(deletedPatient);
        }

        private void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid patient reference.");
            }
        }

        private void ValidatePatient(Patient patient)
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
                throw new BusinessRuleException(
                    "Date of birth cannot be a future date.");
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new BusinessRuleException("Phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new BusinessRuleException("Email address is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                throw new BusinessRuleException("Insurance ID is required.");
            }
        }

        private PatientDto MapToDto(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceId = patient.InsuranceId,
                CreatedDate = patient.CreatedDate
            };
        }

        private List<PatientDto> MapToDtoList(List<Patient> patients)
        {
            List<PatientDto> patientDtos = new List<PatientDto>();

            if (patients == null)
            {
                return patientDtos;
            }

            foreach (Patient patient in patients)
            {
                patientDtos.Add(MapToDto(patient));
            }

            return patientDtos;
        }
    }
}
