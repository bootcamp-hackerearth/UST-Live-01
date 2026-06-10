using HealthAxis.Api.Models;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetByDoctor(int doctorId);
        List<Appointment> GetByPatient(int patientId);
        Appointment GetById(int id);

        void Add(Appointment appointment);

        void Update(Appointment appointment);

        bool ExistsSameDay(int patientId, int doctorId, System.DateTime date);

        void Save();
    }
}