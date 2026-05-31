using HealthCare_Appointments_Portal.Models;

namespace HealthCare_Appointments_Portal.Interfaces
{

    public interface IHealthRecordRepository
    {

        void AddRecord(HealthRecord record);

        List<HealthRecord> GetAllRecords();

        HealthRecord? GetRecordById(int recordId);

        void UpdateRecord(HealthRecord updatedRecord);

        void DeleteRecordById(int recordId);

        /////////////////////////////////////////
        ///
        HealthRecord? GetRecordByAppointmentId(int appointmentId);
        List<HealthRecord> GetRecordsByPatientId(
    int patientId);

        List<HealthRecord> GetRecordsByDoctorId(
            int doctorId);
    }
}