using HealthAxis.Api.Models;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();

        Patient GetById(int id);

        bool ExistsByEmail(string email);

        void Add(Patient patient);

        void Update(Patient patient);

        void Deactivate(int id);
        List<Appointment> GetAppointmentsByPatientId(int patientId);

        void Save();
    }
}