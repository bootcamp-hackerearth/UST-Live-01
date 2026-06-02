using System;
using System.Collections.Generic;
using System.Linq;
using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext? _context;
        private readonly IDoctorRepository? _repository;

        public DoctorService(AppDbContext context)
        {
            _context = context;
        }

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public Doctor AddDoctor(Doctor doctor)
        {
            // The modern .NET 6+ way to check for nulls
            ArgumentNullException.ThrowIfNull(doctor);

            if (string.IsNullOrWhiteSpace(doctor.FullName))
            {
                throw new ArgumentException("Doctor's FullName cannot be empty.", nameof(doctor));
            }

            if (_context == null)
            {
                throw new InvalidOperationException("AppDbContext is not initialized.");
            }

            bool doctorExists = _context.Doctors.Any(
                d => d.FullName.Equals(doctor.FullName, StringComparison.OrdinalIgnoreCase));

            if (doctorExists)
            {
                throw new InvalidOperationException("Doctor already exists.");
            }

            doctor.DoctorId = _context.GetNextDoctorId();
            _context.Doctors.Add(doctor);

            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            if (_context != null)
            {
                return _context.Doctors;
            }

            return _repository?.GetAllDoctors() ?? new List<Doctor>();
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            if (_context != null)
            {
                return _context.Doctors
                    .Where(d => d.Specialisation == specialisation)
                    .ToList();
            }
#pragma warning disable IDE0028 // Simplify collection initialization
            return _repository?.SearchDoctorBySpecialisation(specialisation) ?? new List<Doctor>();
#pragma warning restore IDE0028 // Simplify collection initialization
        }
    }
}