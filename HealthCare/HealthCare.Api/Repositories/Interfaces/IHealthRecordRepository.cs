using HealthCare.Api.Models;

namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IHealthRecordRepository : IRepository<HealthRecord>
    {
       // Task<bool> HealthRecordExistsAsync(int appointmentId);

       // Task<HealthRecord><vw_PatientHealthHistory>>GetPatientHealthHistoryAsync(int patientId,int pageNumber,int pageSize);

       // Task<HealthRecord> GetByAppointmentIdAsync(int appointmentId);
    }
}
