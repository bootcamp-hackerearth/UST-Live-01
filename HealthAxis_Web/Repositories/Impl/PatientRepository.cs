using HealthAxis_Web.Database;
using HealthAxis_Web.Models;
using System.Collections.Generic;
using System.Linq;
using System;

public class PatientRepositoryImpl : IPatientRepository
{
    private readonly AppDBContext _context;

    public PatientRepositoryImpl(AppDBContext context)
    {
        _context = context;
    }

    public List<Patient> GetAllPatients()
    {
        return _context.Patients.ToList();
    }

    public Patient GetById(int id)
    {
        return _context.Patients.FirstOrDefault(p => p.PatientId == id);
    }

    public Patient AddPatient(Patient patient)
    {
        _context.Patients.Add(patient);
        _context.SaveChanges();

        return patient;
    }

    public Patient UpdatePatient(int id, Patient patient)
    {
        var existing = _context.Patients.FirstOrDefault(p => p.PatientId == id);

        if (existing == null)
            return null;

        existing.FullName = patient.FullName;
        existing.DateOfBirth = patient.DateOfBirth;
        existing.Gender = patient.Gender;
        existing.PhoneNumber = patient.PhoneNumber;
        existing.Email = patient.Email;
        existing.InsuranceID = patient.InsuranceID;
        existing.CreatedDate = patient.CreatedDate;

        _context.SaveChanges();

        return existing;
    }
}