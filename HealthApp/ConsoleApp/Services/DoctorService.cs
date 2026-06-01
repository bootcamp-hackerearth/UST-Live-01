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

        public string AddDoctor(Doctor doctor)
        {
            List<Doctor> doctors = _doctorRepo.GetAllDoctors();
            doctor.DoctorId = DoctorIdGenerator(doctors);

            return _doctorRepo.AddDoctor(doctor);
        }

        public Doctor GetDoctorById(int id)
        {
            Doctor? doctor = _doctorRepo.GetDoctorById(id);

            if (doctor is null)
            {
                throw new DoctorNotFoundException($"Doctor of ID {id} does not exist");
            }
            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            var doctors = _doctorRepo.GetAllDoctors();

            if (doctors == null || doctors.Count == 0)
            {
                throw new DoctorNotFoundException("No doctors found.");
            }

            return doctors;
        }

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

        
        public static int DoctorIdGenerator(List<Doctor> doctors)
        {
            return doctors.Count > 0
                ? doctors.Max(d => d.DoctorId) + 1
                : 101;
        }
    }
}
