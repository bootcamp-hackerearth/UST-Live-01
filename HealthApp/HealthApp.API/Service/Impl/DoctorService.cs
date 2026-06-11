using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constant;
using HealthApp.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repo;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ✅ CREATE
        public async Task AddDoctor(DoctorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new Exception("Doctor name is required");

            var doctor = _mapper.Map<Doctor>(dto);
            doctor.IsActive = true;

            await _repo.AddAsync(doctor);
        }

        // ✅ GET ALL
        public async Task<List<DoctorDto>> GetAllDoctors()
        {
            var list = await _repo.GetAllAsync();
            return _mapper.Map<List<DoctorDto>>(list);
        }

        // ✅ GET BY ID
        public async Task<DoctorDto> GetDoctorById(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);

            if (doctor == null)
                throw new Exception($"Doctor with id {id} not found");

            return _mapper.Map<DoctorDto>(doctor);
        }

        // ✅ SEARCH
        public async Task<List<DoctorDto>> SearchBySpecialisation(SpecialisationType specialisation)
        {
            string spec = specialisation.ToString();

            var list = await _repo.GetAllAsync();

            var result = list
                .Where(d => d.Specialisation == spec)
                .ToList();

            return _mapper.Map<List<DoctorDto>>(result);
        }

        // ✅ TOGGLE STATUS
        public async Task ChangeDoctorStatus(int id)
        {
            var doctor = await _repo.GetByIdAsync(id);

            if (doctor == null)
                throw new Exception("Doctor not found");

            doctor.IsActive = !(doctor.IsActive);

            await _repo.UpdateAsync(doctor);
        }
    }
}