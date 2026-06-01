using System;
using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.ConsoleApp.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDb _doctorDb;

        public DoctorRepository(DoctorDb doctorDb)
        {
            _doctorDb = doctorDb;
        }
        public string AddDoctor(Doctor doctor)
        {
            _doctorDb.Doctors.Add(doctor);
            return $"Doctor ID {doctor.DoctorId} added successfully!";
        }
        public Doctor? GetDoctorById(int id)
        {
            return _doctorDb.Doctors.FirstOrDefault(d => d.DoctorId == id);
        }
        public List<Doctor> GetDoctorsBySpecialisation(string specialisation)
        {
            return _doctorDb.Doctors
                .Where(d => d.Specialisation.Equals(specialisation, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Doctor UpdateDoctor(Doctor existingDoctor, Doctor doctor)
        {
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.AvailableSlots = doctor.AvailableSlots;

            return existingDoctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorDb.Doctors.ToList();
        }
    }
}