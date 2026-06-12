using AutoMapper;
using HealthAppMVC.Enums;
using HealthAppWebAPI.Enums;
using HealthAppWebAPI.Models.Dtos;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Services.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients =
                await _repo.GetAllAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int id)
        {
            var patient =
                await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task RegisterPatientAsync(CreatePatientDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Patient data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException("Patient name is required.");
            }

            if (dto.DateOfBirth == default(DateTime))
            {
                throw new ArgumentException("Date of birth is required.");
            }

            if (dto.DateOfBirth > DateTime.Today)
            {
                throw new InvalidOperationException(
                    "Future date is not allowed.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.");
            }

            bool validGender =
                Enum.TryParse(
                    dto.Gender,
                    true,
                    out GenderType gender);

            if (!validGender)
            {
                throw new ArgumentException("Invalid gender.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            string email =
                dto.Email.Trim();

            bool emailExists =
                await _repo.EmailExistsAsync(email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A patient with this email already exists.");
            }

            var patient =
                _mapper.Map<Patient>(dto);

            patient.Email = email;
            patient.Gender = gender.ToString();
            patient.CreatedDate = DateTime.Now;

            await _repo.AddAsync(patient);
        }

        public async Task UpdatePatientAsync(
            int id,
            CreatePatientDto dto)
        {
            var patient =
                await _repo.GetByIdAsync(id);

            if (patient == null)
            {
                throw new KeyNotFoundException("Patient not found.");
            }

            if (dto == null)
            {
                throw new ArgumentException("Patient data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException("Patient name is required.");
            }

            if (dto.DateOfBirth == default(DateTime))
            {
                throw new ArgumentException("Date of birth is required.");
            }

            if (dto.DateOfBirth > DateTime.Today)
            {
                throw new InvalidOperationException(
                    "Future date is not allowed.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new ArgumentException("Gender is required.");
            }

            bool validGender =
                Enum.TryParse(
                    dto.Gender,
                    true,
                    out GenderType gender);

            if (!validGender)
            {
                throw new ArgumentException("Invalid gender.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            string email =
                dto.Email.Trim();

            bool emailUsedByAnotherPatient =
                await _repo.EmailExistsForOtherPatientAsync(
                    id,
                    email);

            if (emailUsedByAnotherPatient)
            {
                throw new InvalidOperationException(
                    "Another patient already uses this email.");
            }

            _mapper.Map(dto, patient);

            patient.Email = email;
            patient.Gender = gender.ToString();

            await _repo.UpdateAsync(patient);
        }
    }
}