using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{
    // Service interface for health record-related operations
    public interface IHealthRecordService
    {
        string AddHealthRecord(HealthRecord record);
        HealthRecord UpdateHealthRecord(HealthRecord record);
        HealthRecord? GetRecordById(int recordId);
        List<HealthRecord> GetByPatientIdOrderByVisitDateDesc(int id);
        List<HealthRecord> GetByDoctorIdOrderByVisitDateDesc(int id);
        List<HealthRecord> GetAllHealthRecords();

    }
}