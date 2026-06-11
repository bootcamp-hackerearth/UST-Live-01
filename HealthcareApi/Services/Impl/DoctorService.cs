using AutoMapper;
using HealthcareApi.Exceptions;
using HealthcareApi.Models;
using HealthcareApi.Repositories;
using HealthcareApi.Repositories.Implementations;
using SharedClasses.Dtos;
using SharedClasses.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HealthcareApi.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        private const string FullNamePattern = @"^[A-Za-z ]+$";

        public DoctorService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentrepository, IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository= appointmentrepository;
            _mapper = mapper;
        }

        public List<DoctorDto> GetAllDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetAll();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public List<DoctorDto> GetAllActiveDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetAllActive();

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public DoctorDto GetDoctorById(int doctorId)
        {
            ValidateDoctorId(doctorId);

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return _mapper.Map<DoctorDto>(doctor);
        }
        public List<DoctorDto> SearchDoctors(string query)
        {
            List<Doctor> doctors = _doctorRepository.SearchDoctors(query);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public List<DoctorDto> SearchDoctorsBySpecialisation(Specialisation specialisation)
        {
            List<Doctor> doctors = _doctorRepository.GetBySpecialisation(specialisation);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }
        public List<DoctorDto> SearchActiveDoctors(string query,
            Specialisation? specialisation)
        {
            List<Doctor> doctors =
                _doctorRepository.SearchActiveDoctors(query, specialisation);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }


        public DoctorDto AddDoctor(CreateDoctorDto dto)
        {
            if (dto == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            NormalizeCreateDoctorDto(dto);

            Doctor doctor = _mapper.Map<Doctor>(dto);

            doctor.IsActive = true;
            doctor.PracticeStartDate = doctor.PracticeStartDate.Date;

            ValidateDoctor(doctor);

            Doctor savedDoctor = _doctorRepository.Add(doctor);

            return _mapper.Map<DoctorDto>(savedDoctor);
        }

        public DoctorDto UpdateDoctor(int doctorId, UpdateDoctorDto dto)
        {
            ValidateDoctorId(doctorId);

            if (dto == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            NormalizeUpdateDoctorDto(dto);

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

            return _mapper.Map<DoctorDto>(updatedDoctor);
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

            bool hasConfirmedAppointmentToday =
            _appointmentRepository.HasConfirmedAppointmentForDoctorOnDate(
                doctorId,
                DateTime.Today);

            if (hasConfirmedAppointmentToday)
            {
                throw new BusinessRuleException(
                "Doctor cannot be deactivated because there are confirmed appointments scheduled for today");
            }
            ValidateDoctorCanBeDeactivated(doctorId);

            doctor.IsActive = false;
            Doctor updatedDoctor = _doctorRepository.Update(doctorId, doctor);

            if (updatedDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return _mapper.Map<DoctorDto>(updatedDoctor);
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

            return _mapper.Map<DoctorDto>(updatedDoctor);
        }

        private void NormalizeCreateDoctorDto(CreateDoctorDto dto)
        {
            dto.FullName = dto.FullName?.Trim();
        }

        private void NormalizeUpdateDoctorDto(UpdateDoctorDto dto)
        {
            dto.FullName = dto.FullName?.Trim();
        }

        private void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Valid Doctor ID is required.");
            }
        }

        private void ValidateDoctorCanBeDeactivated(int doctorId)
        {
            bool hasConfirmedAppointmentToday =
                _appointmentRepository.HasConfirmedAppointmentForDoctorOnDate(
                    doctorId,
                    DateTime.Today);

            if (hasConfirmedAppointmentToday)
            {
                throw new BusinessRuleException(
                    "Doctor cannot be deactivated because there are confirmed appointments scheduled for today.");
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

            if (!Regex.IsMatch(doctor.FullName, FullNamePattern))
            {
                throw new BusinessRuleException("Full name can contain only letters and spaces.");
            }

            if (doctor.PracticeStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (doctor.ConsultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }

            if (doctor.ConsultationFee > 100000)
            {
                throw new BusinessRuleException("Consultation fee cannot exceed 100000.");
            }
        }
    }
}