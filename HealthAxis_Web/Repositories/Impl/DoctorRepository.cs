using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class DoctorRepositoryImpl : IDoctorRepository
    {
        private readonly AppDBContext _context;

        public DoctorRepositoryImpl(AppDBContext context)
        {
            _context = context;
        }

        public List<Doctor> GetAll()
        {
            return _context.Doctors.ToList();
        }

        public Doctor GetById(int id)
        {
            return _context.Doctors.FirstOrDefault(d => d.DoctorId == id);
        }

        public List<Doctor> GetBySpecialisation(string specialisation)
        {
            return _context.Doctors
                .Where(d => d.Specialisation == specialisation)
                .ToList();
        }

        public void Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
        }

        public void Update(Doctor doctor)
        {
            _context.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}