using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared;
using HealthAxis.Shared.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class DoctorServiceImpl : IDoctorService
    {
        private readonly IDoctorRepository _repo;

        public DoctorServiceImpl(IDoctorRepository repo)
        {
            _repo = repo;
        }

        public List<DoctorDto> GetAll(string specialisation)
        {
            List<Doctor> doctors;

            if (!string.IsNullOrEmpty(specialisation))
                doctors = _repo.GetAll()
    .Where(d => d.Specialisation == specialisation)
    .ToList();
            else
                doctors = _repo.GetAll();

            return doctors.Select(d => new DoctorDto
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName,
                Specialisation = d.Specialisation,
                YearsOfExperience = d.YearsOfExperience,
                ConsultationFee = d.ConsultationFee,
                IsActive = d.IsActive
            }).ToList();
        }

        public DoctorDto GetById(int id)
        {
            var d = _repo.GetById(id);

            if (d == null) return null;

            return new DoctorDto
            {
                DoctorId = d.DoctorId,
                FullName = d.FullName,
                Specialisation = d.Specialisation,
                YearsOfExperience = d.YearsOfExperience,
                ConsultationFee = d.ConsultationFee,
                IsActive = d.IsActive
            };
        }

        public void Add(CreateDoctorDto dto)
        {
            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            _repo.Add(doctor);
            _repo.Save();
        }

        public void Update(int id, UpdateDoctorDto dto)
        {
            var doctor = _repo.GetById(id);

            if (doctor == null) return;

            doctor.FullName = dto.FullName;
            doctor.Specialisation = dto.Specialisation;
            doctor.YearsOfExperience = dto.YearsOfExperience;
            doctor.ConsultationFee = dto.ConsultationFee;
            doctor.IsActive = dto.IsActive;

            _repo.Update(doctor);
            _repo.Save();
        }
    }
}