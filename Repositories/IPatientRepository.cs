using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories
{
    public interface IPatientRepository
    {
        void Add(Patient patient);

        Patient GetById(int patientId);

        List<Patient> GetAll();

        bool Update(Patient patient);

        bool Delete(int patientId);
    }
}