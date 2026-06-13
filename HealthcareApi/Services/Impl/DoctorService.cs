using SharedClasses.Dtos;
using SharedClasses.Enums;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using System;
using System.Collections.Generic;

namespace HealthcareApi.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<DoctorDto> GetAllDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetAll();

            return MapToDtoList(doctors);
        }

        public List<DoctorDto> GetAllActiveDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetAllActive();

            return MapToDtoList(doctors);
        }

        public DoctorDto GetDoctorById(int doctorId)
        {
            ValidateDoctorId(doctorId);

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return MapToDto(doctor);
        }

        public List<DoctorDto> SearchDoctorsBySpecialisation(Specialisation specialisation)
        {
            List<Doctor> doctors = _doctorRepository.GetBySpecialisation(specialisation);

            return MapToDtoList(doctors);
        }

        public DoctorDto AddDoctor(CreateDoctorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            Doctor doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                PracticeStartDate = dto.PracticeStartDate.Date,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            ValidateDoctor(doctor);

            Doctor savedDoctor = _doctorRepository.Add(doctor);

            return MapToDto(savedDoctor);
        }

        public DoctorDto UpdateDoctor(int doctorId, UpdateDoctorDto dto)
        {
            ValidateDoctorId(doctorId);

            if (dto == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            Doctor existingDoctor = _doctorRepository.GetById(doctorId);

            if (existingDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            existingDoctor.FullName = dto.FullName;
            existingDoctor.Specialisation = dto.Specialisation;
            existingDoctor.PracticeStartDate = dto.PracticeStartDate.Date;
            existingDoctor.ConsultationFee = dto.ConsultationFee;
            existingDoctor.IsActive = dto.IsActive;

            ValidateDoctor(existingDoctor);

            Doctor updatedDoctor = _doctorRepository.Update(doctorId, existingDoctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return MapToDto(updatedDoctor);
        }

        public DoctorDto DeactivateDoctor(int doctorId)
        {
            ValidateDoctorId(doctorId);

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is already inactive.");
            }

            doctor.IsActive = false;

            Doctor updatedDoctor = _doctorRepository.Update(doctorId, doctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return MapToDto(updatedDoctor);
        }

        public DoctorDto ReactivateDoctor(int doctorId)
        {
            ValidateDoctorId(doctorId);

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            if (doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is already active.");
            }

            doctor.IsActive = true;

            Doctor updatedDoctor = _doctorRepository.Update(doctorId, doctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return MapToDto(updatedDoctor);
        }

        private void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private void ValidateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            if (string.IsNullOrWhiteSpace(doctor.FullName))
            {
                throw new BusinessRuleException("Doctor full name is required.");
            }

            if (doctor.PracticeStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (doctor.ConsultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }
        }

        private DoctorDto MapToDto(Doctor doctor)
        {
            if (doctor == null)
            {
                return null;
            }

            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                PracticeStartDate = doctor.PracticeStartDate,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }

        private List<DoctorDto> MapToDtoList(List<Doctor> doctors)
        {
            List<DoctorDto> doctorDtos = new List<DoctorDto>();

            if (doctors == null)
            {
                return doctorDtos;
            }

            foreach (Doctor doctor in doctors)
            {
                doctorDtos.Add(MapToDto(doctor));
            }

            return doctorDtos;
        }
    }
}