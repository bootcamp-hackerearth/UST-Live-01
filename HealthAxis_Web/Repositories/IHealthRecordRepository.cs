using HealthAxis.Api.Models;
using System.Collections.Generic;

public interface IHealthRecordRepository
{
    void Add(HealthRecord record);
    void Save();
    bool ExistsByAppointment(int appointmentId);
    List<HealthRecord> GetByPatient(int patientId);
}