using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repository;
using HAP_Pod4_ConsoleApp_au.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HAP_Pod4_ConsoleApp_au.Services.Impl
{
    public class PatientService(IPatientRepository repository) : IPatientService
    {
        private readonly IPatientRepository _repository = repository;

        public List<Patient> GetAllPatients()
        {
            return _repository.GetAllPatients();
        }

        public Patient? GetPatientById(int patientId)
        {
            return _repository.GetPatientById(patientId);
        }
        public Patient RegisterPatient(Patient patient)
        {
            return _repository.RegisterPatient(patient);
        }
    }
}
