using AutoMapper;
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
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public List<PatientDto> GetAllPatients()
        {
            List<Patient> patients = _patientRepository.GetAll();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        public PatientDto GetPatientById(int patientId)
        {
            ValidatePatientId(patientId);

            Patient patient = _patientRepository.GetById(patientId);

            if (patient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public PatientDto RegisterPatient(CreatePatientDto dto)
        {
            if (dto == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            Patient patient = _mapper.Map<Patient>(dto);

            patient.CreatedDate = DateTime.Today;

            ValidatePatient(patient);

            Patient savedPatient = _patientRepository.Add(patient);

            return _mapper.Map<PatientDto>(savedPatient);
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

            existingPatient.FullName = dto.FullName;
            existingPatient.DateOfBirth = dto.DateOfBirth.Date;
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

            return _mapper.Map<PatientDto>(updatedPatient);
        }

        public PatientDto DeletePatient(int patientId)
        {
            ValidatePatientId(patientId);

            Patient deletedPatient = _patientRepository.Delete(patientId);

            if (deletedPatient == null)
            {
                throw new EntityNotFoundException("Patient", patientId);
            }

            return _mapper.Map<PatientDto>(deletedPatient);
        }

        private void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Valid Patient ID is required.");
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
    }
}