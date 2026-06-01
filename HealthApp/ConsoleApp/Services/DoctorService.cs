using System.Collections.Generic;
using System.Linq;
using HealthApp.ConsoleApp.Interfaces;
using HealthApp.ConsoleApp.Models;
using HealthApp.ConsoleApp.Exceptions;

namespace HealthApp.ConsoleApp.Services
{
    // Service class to manage doctors in the healthcare system
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepo = doctorRepository;
        }
        // Method to add a new doctor to the database
        public string AddDoctor(Doctor doctor)
        {
            List<Doctor> doctors = _doctorRepo.GetAllDoctors();
            doctor.DoctorId = DoctorIdGenerator(doctors);

            return _doctorRepo.AddDoctor(doctor);
        }
        // Method to get a doctor by ID from the database
        public Doctor? GetDoctorById(int id)
        {
            Doctor? doctor = _doctorRepo.GetDoctorById(id);

            if (doctor is null)
            {
                throw new DoctorNotFoundException($"Doctor of ID {id} does not exist");
            }
            return doctor;
        }
        // Method to get doctors by specialisation from the database
        public List<Doctor> GetDoctorsBySpecialisation(string specialisation)
        {
            var result = _doctorRepo.GetDoctorsBySpecialisation(specialisation);

            if (result == null || result.Count == 0)
            {
                throw new SpecialisationNotFoundException($"Doctor with specialisation in {specialisation} does not exist");
            }

            return result;
        }

        public Doctor UpdateDoctor(Doctor doctor)
        {
            Doctor? existingDoctor = GetDoctorById(doctor.DoctorId);

            if (existingDoctor is null)
            {
                throw new DoctorNotFoundException($"Doctor of ID {doctor.DoctorId} does not exist");
            }
            return _doctorRepo.UpdateDoctor(existingDoctor, doctor);
        }

        // Method to generate a unique doctor ID based on existing doctors in the database        
        public static int DoctorIdGenerator(List<Doctor> doctors)
        {
            return doctors.Any()
                ? doctors.Max(d => d.DoctorId) + 1
                : 201;
        }
        // Method to get all doctors from the database
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepo.GetAllDoctors();
        }
    }
}