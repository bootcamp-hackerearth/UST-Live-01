using System.Collections.Generic;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories
{
    public interface IHealthRecordRepository
    {
        void Add(HealthRecord record);

        HealthRecord GetById(int recordId);

        List<HealthRecord> GetAll();

        List<HealthRecord> GetByPatientId(int patientId);

        List<HealthRecord> GetByDoctorId(int doctorId);

        List<HealthRecord> GetByAppointmentId(int appointmentId);

        bool ExistsByAppointmentId(int appointmentId);

        bool Update(HealthRecord record);

        bool Delete(int recordId);
    }
}