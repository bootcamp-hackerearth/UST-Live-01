using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Repositories.Interfaces;

namespace Healthaxis2.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext db;

        public DoctorRepository(AppDbContext context)
        {
            db = context;
        }

        public List<Doctor> GetAll()
        {
            return db.Doctors.Where(d => d.IsActive).ToList();
        }

        public Doctor GetById(int id)
        {
            return db.Doctors.Find(id);
        }

        public void Add(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            db.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            db.Entry(doctor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
            db.SaveChanges();
        }
    }
}