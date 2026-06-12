using System.Collections.Generic;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;
using Healthaxis2.Services.Interfaces;

namespace Healthaxis2.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext db;

        public PatientService(AppDbContext context)
        {
            db = context;
        }

        public List<Patient> GetAll() => db.Patients.ToList();

        public Patient GetById(int id) => db.Patients.Find(id);

        public Patient Create(Patient patient)
        {
            db.Patients.Add(patient);
            db.SaveChanges();
            return patient;
        }

        public void Update(int id, Patient patient)
        {
            var existing = db.Patients.Find(id);
            db.Entry(existing).CurrentValues.SetValues(patient);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var p = db.Patients.Find(id);
            db.Patients.Remove(p);
            db.SaveChanges();
        }
    }
}