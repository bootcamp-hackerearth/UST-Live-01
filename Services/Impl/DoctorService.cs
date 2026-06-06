using System;
using System.Collections.Generic;
using System.Linq;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Exceptions;
using HealthcareMvcApp.Models;
using HealthcareMvcApp.Repositories;

namespace HealthcareMvcApp.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);

            doctor.IsActive = true;

            if (doctor.OffDays == null)
            {
                doctor.OffDays = new List<DateTime>();
            }

            doctor.OffDays = doctor.OffDays
                .Select(d => d.Date)
                .Distinct()
                .ToList();

            _doctorRepository.Add(doctor);

            return doctor;
        }
        public List<Doctor> GetAvailableDoctorsBySpecialisation(
            Specialisation specialisation,
            DateTime appointmentDate,
            int slotNumber)
        {
            DateTime selectedDate = appointmentDate.Date;

            if (selectedDate < DateTime.Today)
            {
                throw new BusinessRuleException("Appointment date cannot be in the past.");
            }

            if (slotNumber < 1 || slotNumber > 10)
            {
                throw new BusinessRuleException("Slot number must be between 1 and 10.");
            }

            List<Doctor> doctors = _doctorRepository.GetActiveBySpecialisation(specialisation);

            return doctors
                .Where(d =>
                    d.IsAvailable(selectedDate) &&
                    !_appointmentRepository.IsSlotBooked(d.DoctorId, selectedDate, slotNumber))
                .ToList();
        }
        public Doctor GetDoctorById(int doctorId)
        {
            ValidateDoctorId(doctorId);

            Doctor doctor = _doctorRepository.GetById(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }

        public List<Doctor> GetAllActiveDoctors()
        {
            return _doctorRepository.GetAllActive();
        }

        public List<Doctor> SearchDoctorsBySpecialisation(Specialisation specialisation)
        {
            return _doctorRepository.GetActiveBySpecialisation(specialisation);
        }

        public Doctor UpdateDoctor(Doctor doctor)
        {
            ValidateDoctor(doctor);
            ValidateDoctorId(doctor.DoctorId);

            Doctor existingDoctor = _doctorRepository.GetById(doctor.DoctorId);

            if (existingDoctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctor.DoctorId);
            }

            if (doctor.OffDays == null)
            {
                doctor.OffDays = new List<DateTime>();
            }

            doctor.OffDays = doctor.OffDays
                .Select(d => d.Date)
                .Distinct()
                .ToList();

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                throw new EntityNotFoundException("Doctor", doctor.DoctorId);
            }

            return doctor;
        }

        public Doctor DeactivateDoctor(int doctorId)
        {
            Doctor doctor = GetDoctorById(doctorId);

            if (!doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is already inactive.");
            }

            doctor.IsActive = false;

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        public List<DateTime> GetOffDays(int doctorId)
        {
            Doctor doctor = GetDoctorById(doctorId);

            if (doctor.OffDays == null)
            {
                return new List<DateTime>();
            }

            return doctor.OffDays
                .Select(d => d.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        public Doctor AddOffDay(int doctorId, DateTime offDay)
        {
            Doctor doctor = GetDoctorById(doctorId);

            DateTime selectedDate = offDay.Date;

            if (selectedDate <= DateTime.Today)
            {
                throw new BusinessRuleException("Selected off day must be in the future.");
            }

            if (doctor.OffDays == null)
            {
                doctor.OffDays = new List<DateTime>();
            }

            bool alreadyExists = doctor.OffDays.Any(d => d.Date == selectedDate);

            if (alreadyExists)
            {
                throw new BusinessRuleException("This date is already marked as an off day.");
            }

            bool hasConfirmedAppointments = _appointmentRepository
                .GetByDoctorId(doctorId)
                .Any(a =>
                    a.ScheduledDate.Date == selectedDate &&
                    a.Status == AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments)
            {
                throw new AppointmentRuleException(
                    "Cannot mark this date as off because the doctor has confirmed appointments on this date.");
            }

            doctor.OffDays.Add(selectedDate);

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        public Doctor RemoveOffDay(int doctorId, DateTime offDay)
        {
            Doctor doctor = GetDoctorById(doctorId);

            DateTime selectedDate = offDay.Date;

            if (doctor.OffDays == null ||
                !doctor.OffDays.Any(d => d.Date == selectedDate))
            {
                throw new BusinessRuleException("This date is not currently marked as an off day.");
            }

            doctor.OffDays.RemoveAll(d => d.Date == selectedDate);

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        public Doctor ReactivateDoctor(int doctorId)
        {
            Doctor doctor = GetDoctorById(doctorId);

            if (doctor.IsActive)
            {
                throw new BusinessRuleException("Doctor is already active.");
            }

            doctor.IsActive = true;

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private static void ValidateDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new BusinessRuleException("Doctor details are required.");
            }

            if (string.IsNullOrWhiteSpace(doctor.FullName))
            {
                throw new BusinessRuleException("Doctor full name is required.");
            }

            if (doctor.PracticeStartDate == DateTime.MinValue)
            {
                throw new BusinessRuleException("Practice start date is required.");
            }

            if (doctor.PracticeStartDate.Date > DateTime.Today)
            {
                throw new BusinessRuleException("Practice start date cannot be in the future.");
            }

            if (doctor.ConsultationFee < 0)
            {
                throw new BusinessRuleException("Consultation fee cannot be negative.");
            }

            if (doctor.OffDays != null)
            {
                int distinctDateCount = doctor.OffDays
                    .Select(d => d.Date)
                    .Distinct()
                    .Count();

                if (distinctDateCount != doctor.OffDays.Count)
                {
                    throw new BusinessRuleException("Doctor off-day dates must be different.");
                }
            }
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Valid Doctor ID is required.");
            }
        }
    }
}