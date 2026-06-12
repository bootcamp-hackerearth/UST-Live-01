using HealthAxis.Api.Models;
using HealthAxis.Api.Repositories;
using HealthAxis.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class PatientServiceImpl : IPatientService
    {
        private readonly IPatientRepository _repo;

        public PatientServiceImpl(IPatientRepository repo)
        {
            _repo = repo;
        }

        public ApiResponseDto Create(CreatePatientDto dto)
        {
            var patient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth.Value,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceId = dto.InsuranceId,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _repo.Add(patient);
            _repo.Save();

            return new ApiResponseDto
            {
                Success = true,
                Message = "Patient created successfully"
            };
        }

        public List<PatientDto> GetAllPatients()
        {
            return _repo.GetAll().Select(p => new PatientDto
            {
                PatientId = p.PatientId,
                FullName = p.FullName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                InsuranceId = p.InsuranceId,
                IsActive = p.IsActive
            }).ToList();
        }

        public PatientDto GetById(int id)
        {
            var patient = _repo.GetById(id);

            if (patient == null)
                return null;

            var dto = new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceId = patient.InsuranceId,
                IsActive = patient.IsActive
            };

            var count = _repo.GetAppointmentsByPatientId(id)
                .Count(a => a.ScheduledDate >= DateTime.Today
                            && a.Status != "Cancelled");

            dto.UpcomingAppointments = count;

            return dto;
        }

        public PatientDto Update(int id, PatientDto dto)
        {
            var patient = _repo.GetById(id);
            if (patient == null) return null;

            patient.FullName = dto.FullName;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.PhoneNumber = dto.PhoneNumber;
            patient.Email = dto.Email;
            patient.InsuranceId = dto.InsuranceId;
            patient.IsActive = dto.IsActive;

            _repo.Update(patient);
            _repo.Save();

            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceId = patient.InsuranceId,
                IsActive = patient.IsActive
            };
        }

        public bool Deactivate(int id)
        {
            var patient = _repo.GetById(id);
            if (patient == null)
                return false;

            _repo.Deactivate(id);
            _repo.Save();

            return true;
        }
    }
}