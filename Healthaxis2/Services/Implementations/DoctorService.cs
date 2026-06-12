using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Services.Interfaces;

namespace Healthaxis2.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext db;

        public DoctorService(AppDbContext context)
        {
            db = context;
        }

        public List<Doctor> GetAll() =>
            db.Doctors.Where(d => d.IsActive).ToList();

        public Doctor GetById(int id) => db.Doctors.Find(id);

        public Doctor Create(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            db.SaveChanges();
            return doctor;
        }

        public void Update(int id, Doctor doctor)
        {
            var existing = db.Doctors.Find(id);
            db.Entry(existing).CurrentValues.SetValues(doctor);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var d = db.Doctors.Find(id);
            db.Doctors.Remove(d);
            db.SaveChanges();
        }
    }
}