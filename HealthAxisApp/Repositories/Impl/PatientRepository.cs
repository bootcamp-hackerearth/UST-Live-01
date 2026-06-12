using HealthAxisApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxisApp.Repositories.Impl
{
    namespace HealthAxisApp.Repositories.Impl
    {
        public class PatientRepository : IPatientRepository
        {
            private readonly HealthAxisEntities2 _context;

            public PatientRepository(HealthAxisEntities2 context)
            {
                _context = context;
            }

            public IEnumerable<Patient> GetAll(string insuranceStatus = null, string searchText = null)
            {
                var query = _context.Patients.AsQueryable();

                if (insuranceStatus == "Insured")
                {
                    query = query.Where(p =>
                        p.InsuranceID != null &&
                        p.InsuranceID != "");
                }

                if (insuranceStatus == "NotInsured")
                {
                    query = query.Where(p =>
                        p.InsuranceID == null ||
                        p.InsuranceID == "");
                }


                if (!string.IsNullOrEmpty(searchText))
                {
                    searchText = searchText.ToLower();

                    query = query.Where(p =>
                        p.FullName.ToLower().Contains(searchText) ||
                        p.PhoneNumber.Contains(searchText));
                }


                return query
                    .OrderBy(p => p.FullName)
                    .ToList();
            }

            public Patient GetById(int id)
            {
                return _context.Patients.Find(id);
            }

            public Patient GetByEmail(string email)
            {
                return _context.Patients
                    .FirstOrDefault(p => p.Email == email);
            }

            public Patient Add(Patient patient)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();

                return patient;
            }

            public bool Update(Patient patient)
            {
                var existingPatient = _context.Patients.Find(patient.PatientId);

                if (existingPatient == null)
                {
                    return false;
                }

                existingPatient.FullName = patient.FullName;
                existingPatient.DateOfBirth = patient.DateOfBirth;
                existingPatient.Gender = patient.Gender;
                existingPatient.PhoneNumber = patient.PhoneNumber;
                existingPatient.Email = patient.Email;
                existingPatient.InsuranceID = patient.InsuranceID;

                _context.SaveChanges();

                return true;
            }

            public bool Delete(int id)
            {
                var patient = _context.Patients.Find(id);

                if (patient == null)
                {
                    return false;
                }

                _context.Patients.Remove(patient);
                _context.SaveChanges();

                return true;
            }

            public int GetAppointmentCount(int patientId)
            {
                return _context.Appointments
                    .Count(a => a.PatientId == patientId);
            }
        }
    }
}