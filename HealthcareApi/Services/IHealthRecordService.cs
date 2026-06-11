using SharedClasses.Dtos;
using System.Collections.Generic;

namespace HealthcareApi.Services
{
    public interface IHealthRecordService
    {
        List<HealthRecordDto> GetAllRecords();

        HealthRecordDto GetRecordById(int healthRecordId);

        List<HealthRecordDto> GetRecordsByPatient(int patientId);

        List<HealthRecordDto> GetRecordsByDoctor(int doctorId);

        List<HealthRecordDto> GetRecordsByAppointment(int appointmentId);

        HealthRecordDto AddRecord(AddHealthRecordDto dto);

        HealthRecordDto UpdateRecord(int healthRecordId, UpdateHealthRecordDto dto);

        HealthRecordDto DeleteRecord(int healthRecordId);

        List<HealthRecordDto> SearchHealthRecords(string query);
        List<HealthRecordDto> SearchHealthRecordsByPatient(
            int patientId,
            string query);

        List<HealthRecordDto> SearchHealthRecordsByDoctor(
            int doctorId,
            string query);
    }
}