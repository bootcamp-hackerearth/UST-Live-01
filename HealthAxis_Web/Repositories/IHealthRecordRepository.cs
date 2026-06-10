using HealthAxis.Api.Models;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetByPatient(int patientId);

        HealthRecord GetById(int id);

        bool ExistsByAppointment(int appointmentId);

        void Add(HealthRecord record);

        void Save();
    }
}