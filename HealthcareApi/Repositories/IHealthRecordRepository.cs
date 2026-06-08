using HealthcareApi.Models;
using System.Collections.Generic;

namespace HealthcareApi.Repositories
{
    public interface IHealthRecordRepository
    {
        List<HealthRecord> GetAll();

        HealthRecord GetById(int healthRecordId);

        List<HealthRecord> GetByPatientId(int patientId);

        List<HealthRecord> GetByDoctorId(int doctorId);

        List<HealthRecord> GetByAppointmentId(int appointmentId);

        bool ExistsByAppointmentId(int appointmentId);

        HealthRecord Add(HealthRecord record);

        HealthRecord Update(int healthRecordId, HealthRecord record);

        HealthRecord Delete(int healthRecordId);
    }
}