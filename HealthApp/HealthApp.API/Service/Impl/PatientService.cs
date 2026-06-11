using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Impl
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ✅ CREATE
        public async Task RegisterPatient(PatientDto dto)
        {
            if (dto == null)
                throw new Exception("Patient cannot be null");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("Patient name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new Exception("Email is required");

            // ✅ async DB call
            var patients = await _repo.GetAllAsync();

            bool emailExists = patients.Any(p =>
                p.Email.ToLower() == dto.Email.ToLower());

            if (emailExists)
                throw new Exception("Email already exists");

            var patient = _mapper.Map<Patient>(dto);
            patient.CreatedDate = DateTime.Now;

            await _repo.AddAsync(patient);
        }

        // ✅ GET ALL
        public async Task<List<PatientDto>> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<PatientDto>>(list);
        }

        // ✅ GET BY ID
        public async Task<PatientDto> GetPatientById(int id)
        {
            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
                throw new Exception($"Patient with id {id} not found");

            return _mapper.Map<PatientDto>(patient);
        }

        // ✅ UPDATE
        public async Task UpdatePatientById(int id, PatientDto dto)
        {
            var existingPatient = await _repo.GetByIdAsync(id);

            if (existingPatient == null)
                throw new Exception($"Patient with id {id} not found");

            var patient = _mapper.Map<Patient>(dto);
            patient.PatientId = id;

            await _repo.UpdatePatientAsync(id, patient);
        }
    }
}