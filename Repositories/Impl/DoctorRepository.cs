using System.Collections.Generic;
using System.Linq;
using HealthcareMvcApp.Data;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthcareDbContext _context;

        public DoctorRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public void Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }

        public Doctor GetById(int doctorId)
        {
            return _context.Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
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
                .Where(d =>
                    d.Specialisation == specialisation &&
                    d.IsActive)
                .OrderBy(d => d.FullName)
                .ToList();
        }

        public bool Update(Doctor doctor)
        {
            Doctor existingDoctor = GetById(doctor.DoctorId);

            if (existingDoctor == null)
            {
                return false;
            }

            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.PracticeStartDate = doctor.PracticeStartDate.Date;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;

            _context.SaveChanges();

            return true;
        }
    }
}