using System.Collections.Generic;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Services
{
    public interface IHealthRecordService
    {
        HealthRecord AddRecord(
            int appointmentId,
            string diagnosis,
            string prescription,
            string notes);

        HealthRecord GetRecordById(int recordId);

        List<HealthRecord> GetAllRecords();

        List<HealthRecord> GetRecordsByPatient(int patientId);

        List<HealthRecord> GetRecordsByDoctor(int doctorId);

        List<HealthRecord> GetRecordsByAppointment(int appointmentId);

        HealthRecord UpdateRecord(HealthRecord record);

        HealthRecord DeleteRecord(int recordId);

        HealthRecord GetRecordForDoctor(int recordId, int doctorId);

        HealthRecord UpdateRecordByDoctor(int doctorId, HealthRecord record);
    }
}