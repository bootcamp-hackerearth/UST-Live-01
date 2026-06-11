using HealthcareApi.Data;
using SharedClasses.Enums;
using HealthcareApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareApi.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthcareDbContext _context;

        public DoctorRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAll()
        {
            return _context.Doctors
                .OrderBy(d => d.Specialisation)
                .ThenBy(d => d.FullName)
                .ToList();
        }

        public List<Doctor> GetAllActive()
        {
            return _context.Doctors
                .Where(d => d.IsActive)
                .OrderBy(d => d.Specialisation)
                .ThenBy(d => d.FullName)
                .ToList();
        }

        public Doctor GetById(int doctorId)
        {
            return _context.Doctors
                .FirstOrDefault(d => d.DoctorId == doctorId);
        }

        public List<Doctor> GetBySpecialisation(Specialisation specialisation)
        {
            return _context.Doctors
                .Where(d => d.Specialisation == specialisation)
                .OrderBy(d => d.FullName)
                .ToList();
        }

        public List<Doctor> GetActiveBySpecialisation(Specialisation specialisation)
        {
            return _context.Doctors
                .Where(d => d.Specialisation == specialisation && d.IsActive)
                .OrderBy(d => d.FullName)
                .ToList();
        }

        public Doctor Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            return doctor;
        }

        public Doctor Update(int doctorId, Doctor doctor)
        {
            Doctor existingDoctor = GetById(doctorId);

            if (existingDoctor == null)
            {
                return null;
            }

            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.PracticeStartDate = doctor.PracticeStartDate.Date;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;

            _context.SaveChanges();

            return existingDoctor;
        }
        public List<Doctor> SearchDoctors(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return GetAll();
            }
            string searchTerm = query.Trim().ToLower();
            return _context.Doctors
                    .Where(d =>
                        d.FullName != null &&
                        d.FullName.ToLower().Contains(searchTerm))
                    .OrderBy(d => d.Specialisation)
                    .ThenBy(d => d.FullName)
                    .ToList();
            
        }

        public List<Doctor> SearchActiveDoctors(string query, Specialisation? specialisation)
        {
            IQueryable<Doctor> doctors = _context.Doctors
                .Where(d => d.IsActive);

            if (specialisation.HasValue)
            {
                doctors = doctors.Where(d => d.Specialisation == specialisation.Value);
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                string searchTerm = query.Trim().ToLower();

                doctors = doctors.Where(d =>
                    d.FullName != null &&
                    d.FullName.ToLower().Contains(searchTerm));
            }

            return doctors
                .OrderBy(d => d.Specialisation)
                .ThenBy(d => d.FullName)
                .ToList();
        }
    }
}