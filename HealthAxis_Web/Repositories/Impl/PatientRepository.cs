using HealthAxis.Api.Database;
using HealthAxis.Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

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
        public List<Appointment> GetAppointmentsByPatientId(int patientId)
        {
            return _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToList();
        }
        public bool ExistsByEmail(string email)
        {
            return _context.Patients.Any(x => x.Email == email);
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
        }

        public void Update(Patient patient)
        {
            _context.Entry(patient).State = EntityState.Modified;
        }

        public void Deactivate(int id)
        {
            var patient = _context.Patients.Find(id);
            if (patient != null)
            {
                patient.IsActive = false;

                _context.Entry(patient).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}