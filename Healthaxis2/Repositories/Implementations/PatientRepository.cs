using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Repositories.Interfaces;

namespace Healthaxis2.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext db;

        public PatientRepository(AppDbContext context)
        {
            db = context;
        }

        public List<Patient> GetAll()
        {
            return db.Patients.ToList();
        }

        public Patient GetById(int id)
        {
            return db.Patients.Find(id);
        }

        public void Add(Patient patient)
        {
            db.Patients.Add(patient);
            db.SaveChanges();
        }

        public void Update(Patient patient)
        {
            db.Entry(patient).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
        }
    }
}