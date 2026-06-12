using HealthAxisApp.Data;
using System.Collections.Generic;

namespace HealthAxisApp.Repositories
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAll(string insuranceStatus = null, string searchText = null);

        Patient GetById(int id);

        Patient GetByEmail(string email);

        Patient Add(Patient patient);

        bool Update(Patient patient);

        bool Delete(int id);

        int GetAppointmentCount(int patientId);


    }
}
