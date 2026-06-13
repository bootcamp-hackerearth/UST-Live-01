using HealthcareApi.Data;
using HealthcareApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareApi.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly HealthcareDbContext _context;

        public PatientRepository(HealthcareDbContext context)
        {
            _context = context;
        }

        public List<Patient> GetAll()
        {
            return _context.Patients
                .OrderBy(p => p.FullName)
                .ToList();
        }

        public bool IsDuplicatePatient(string name, string phone, string email, DateTime dob)
        {
            return _context.Patients.Any(p =>
                (p.FullName.ToLower() == name && p.PhoneNumber == phone)

                ||

                p.Email.ToLower() == email

                ||

                (p.FullName.ToLower() == name && p.DateOfBirth == dob)
            );
        }

        public Patient GetById(int patientId)
        {
            return _context.Patients
                .FirstOrDefault(p => p.PatientId == patientId);
        }

        public Patient Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            return patient;
        }

        public Patient Update(int patientId, Patient patient)
        {
            Patient existingPatient = GetById(patientId);

            if (existingPatient == null)
            {
                return null;
            }

            existingPatient.FullName = patient.FullName;
            existingPatient.DateOfBirth = patient.DateOfBirth.Date;
            existingPatient.Gender = patient.Gender;
            existingPatient.PhoneNumber = patient.PhoneNumber;
            existingPatient.Email = patient.Email;
            existingPatient.InsuranceId = patient.InsuranceId;

            _context.SaveChanges();

            return existingPatient;
        }

        public Patient Delete(int patientId)
        {
            Patient existingPatient = GetById(patientId);

            if (existingPatient == null)
            {
                return null;
            }

            _context.Patients.Remove(existingPatient);
            _context.SaveChanges();

            return existingPatient;
        }
    }
}