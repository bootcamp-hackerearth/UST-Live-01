using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Services.Interfaces
{
    public interface IPatientService
    {
        List<Patient> GetAll();
        Patient GetById(int id);
        Patient Create(Patient patient);
        void Update(int id, Patient patient);
        void Delete(int id);
    }
}