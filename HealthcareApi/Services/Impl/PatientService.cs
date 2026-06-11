using AutoMapper;
using SharedClasses.Dtos;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HealthcareApi.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        private const string FullNamePattern = @"^[A-Za-z ]+$";
        private const string PhoneNumberPattern = @"^\d{10}$";
        private const string InsuranceIdPattern = @"^INS\d+$";

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
        public List<PatientDto> SearchPatients(string query)
        {
            List<Patient> patients = _patientRepository.SearchPatients(query);

            return _mapper.Map<List<PatientDto>>(patients);
        }
        public PatientDto RegisterPatient(CreatePatientDto dto)
        {
            if (dto == null)
            {
                throw new BusinessRuleException("Patient details are required.");
            }

            NormalizeCreatePatientDto(dto);

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

            NormalizeUpdatePatientDto(dto);

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

        private void NormalizeCreatePatientDto(CreatePatientDto dto)
        {
            dto.FullName = dto.FullName?.Trim();
            dto.Email = dto.Email?.Trim();
            dto.PhoneNumber = dto.PhoneNumber?.Trim();
            dto.InsuranceId = dto.InsuranceId?.Trim().ToUpper();
        }

        private void NormalizeUpdatePatientDto(UpdatePatientDto dto)
        {
            dto.FullName = dto.FullName?.Trim();
            dto.Email = dto.Email?.Trim();
            dto.PhoneNumber = dto.PhoneNumber?.Trim();
            dto.InsuranceId = dto.InsuranceId?.Trim().ToUpper();
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

            if (!Regex.IsMatch(patient.FullName, FullNamePattern))
            {
                throw new BusinessRuleException("Full name can contain only letters and spaces.");
            }

            if (patient.DateOfBirth.Date < new DateTime(1900, 1, 1))
            {
                throw new BusinessRuleException("Date of birth cannot be before 01 Jan 1900.");
            }

            if (patient.DateOfBirth.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Date of birth cannot be in the future.");
            }

            if (string.IsNullOrWhiteSpace(patient.PhoneNumber))
            {
                throw new BusinessRuleException("Phone number is required.");
            }

            if (!Regex.IsMatch(patient.PhoneNumber, PhoneNumberPattern))
            {
                throw new BusinessRuleException("Phone number must be exactly 10 digits.");
            }

            if (string.IsNullOrWhiteSpace(patient.Email))
            {
                throw new BusinessRuleException("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(patient.InsuranceId))
            {
                throw new BusinessRuleException("Insurance ID is required.");
            }

            if (!Regex.IsMatch(patient.InsuranceId, InsuranceIdPattern))
            {
                throw new BusinessRuleException(
                    "Insurance ID must start with INS followed by digits only. Example: INS12345");
            }
        }
    }
}