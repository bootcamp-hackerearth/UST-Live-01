using System.Collections.Generic;
using System.Linq;
using HealthcareMvcApp.Data;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthcareDbContext _context;

        public PatientRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public Patient GetById(int patientId)
        {
            return _context.Patients.FirstOrDefault(p => p.PatientId == patientId);
        }

        public List<Patient> GetAll()
        {
            return _context.Patients
                .OrderBy(p => p.FullName)
                .ToList();
        }

        public bool Update(Patient patient)
        {
            Patient existingPatient = GetById(patient.PatientId);

            if (existingPatient == null)
            {
                return false;
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth.Date;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;
            existingPatient.CreatedDate = patient.CreatedDate.Date;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int patientId)
        {
            Patient existingPatient = GetById(patientId);

            if (existingPatient == null)
            {
                return false;
            }

            _context.Patients.Remove(existingPatient);
            _context.SaveChanges();

            return true;
        }
    }
}