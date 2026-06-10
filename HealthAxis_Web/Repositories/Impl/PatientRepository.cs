using HealthAxis.Api.Models;
using System.Collections.Generic;
using System.Linq;
using HealthAxis.Api.Database;

namespace HealthAxis.Api.Repositories
{
    public class PatientRepositoryImpl : IPatientRepository
    {
        private readonly AppDBContext _context;

        public PatientRepositoryImpl(AppDBContext context)
        {
            _context = context;
        }

        public List<Patient> GetAll()
        {
            return _context.Patients.ToList();
        }

        public Patient GetById(int id)
        {
            return _context.Patients.Find(id);
        }

        public bool ExistsByEmail(string email)
        {
            return _context.Patients.Any(p => p.Email == email);
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
        }

        public void Update(Patient patient)
        {
            _context.Entry(patient).State =
                System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}