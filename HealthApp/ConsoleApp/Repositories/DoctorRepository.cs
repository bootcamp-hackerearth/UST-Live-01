using System;
using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Databases;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.ConsoleApp.Repositories
{
    // Repository class to manage doctors in the healthcare system
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DoctorDb _doctorDb;

        public DoctorRepository(DoctorDb doctorDb)
        {
            _doctorDb = doctorDb;
        }
        // Method to add a new doctor to the database
        public string AddDoctor(Doctor doctor)
        {
            _doctorDb.Doctors.Add(doctor);
            return $"Doctor ID {doctor.DoctorId} added successfully!";
        }
        // Method to get a doctor by ID from the database
        public Doctor? GetDoctorById(int id)
        {
            return _doctorDb.Doctors.FirstOrDefault(d => d.DoctorId == id);
        }
        // Method to get doctors by specialisation from the database
        public List<Doctor> GetDoctorsBySpecialisation(string specialisation)
        {
            return _doctorDb.Doctors
                .Where(d => d.Specialisation.Equals(specialisation, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        // Method to update an existing doctor in the database
        public Doctor UpdateDoctor(Doctor existingDoctor, Doctor doctor)
        {
            existingDoctor.Name = doctor.Name;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.AvailableDates=doctor.AvailableDates;
            existingDoctor.AvailableSlots=doctor.AvailableSlots;

            return existingDoctor;
        }
        // Method to get all doctors from the database
        public List<Doctor> GetAllDoctors()
        {
            return _doctorDb.Doctors.ToList();
        }
    }
}