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
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<DoctorDto>> GetAllDoctorsAsync()
        {
            var doctors =
                await _repo.GetAllAsync();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<DoctorDto> GetDoctorByIdAsync(int id)
        {
            var doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task AddDoctorAsync(CreateDoctorDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentException("Doctor data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException("Doctor name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Specialisation))
            {
                throw new ArgumentException("Specialisation is required.");
            }

            bool validSpecialisation =
                Enum.TryParse(
                    dto.Specialisation,
                    true,
                    out SpecialisationType specialisation);

            if (!validSpecialisation)
            {
                throw new ArgumentException("Invalid Specialisation.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorEmail))
            {
                throw new ArgumentException("Doctor email is required.");
            }

            string email =
                dto.DoctorEmail.Trim();

            bool emailExists =
                await _repo.EmailExistsAsync(email);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A doctor with this email already exists.");
            }

            var doctor =
                _mapper.Map<Doctor>(dto);

            doctor.DoctorEmail = email;
            doctor.Specialisation = specialisation.ToString();
            doctor.IsActive = true;

            await _repo.AddAsync(doctor);
        }

        public async Task UpdateDoctorAsync(
            int id,
            CreateDoctorDto dto)
        {
            var doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            if (dto == null)
            {
                throw new ArgumentException("Doctor data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.FullName))
            {
                throw new ArgumentException("Doctor name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Specialisation))
            {
                throw new ArgumentException("Specialisation is required.");
            }

            bool validSpecialisation =
                Enum.TryParse(
                    dto.Specialisation,
                    true,
                    out SpecialisationType specialisation);

            if (!validSpecialisation)
            {
                throw new ArgumentException("Invalid Specialisation.");
            }

            if (string.IsNullOrWhiteSpace(dto.DoctorEmail))
            {
                throw new ArgumentException("Doctor email is required.");
            }

            string email =
                dto.DoctorEmail.Trim();

            bool emailUsedByAnotherDoctor =
                await _repo.EmailExistsForOtherDoctorAsync(
                    id,
                    email);

            if (emailUsedByAnotherDoctor)
            {
                throw new InvalidOperationException(
                    "Another doctor already uses this email.");
            }

            _mapper.Map(dto, doctor);

            doctor.DoctorEmail = email;
            doctor.Specialisation = specialisation.ToString();

            await _repo.UpdateAsync(doctor);
        }

        public async Task ChangeStatusAsync(
            int id,
            bool isActive)
        {
            var doctor =
                await _repo.GetByIdAsync(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            await _repo.ChangeStatusAsync(id, isActive);
        }

        public async Task<List<DoctorDto>> GetDoctorsBySpecialisationAsync(
            string specialisation)
        {
            if (string.IsNullOrWhiteSpace(specialisation))
            {
                throw new ArgumentException("Specialisation is required.");
            }

            bool validSpecialisation =
                Enum.TryParse(
                    specialisation,
                    true,
                    out SpecialisationType spec);

            if (!validSpecialisation)
            {
                throw new ArgumentException("Invalid Specialisation.");
            }

            var doctors =
                await _repo.GetBySpecialisationAsync(spec);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }
    }
}