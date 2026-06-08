using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IHealthRecordRepository
        : IRepository<HealthRecord>
    {
        Task<bool>
            RecordExistsAsync(
                int appointmentId);

        Task<IEnumerable<HealthRecord>>
            GetRecordsByPatientAsync(
                int patientId);

        Task<IEnumerable<HealthRecord>>
            GetRecordsByDoctorAsync(
                int doctorId);

        Task<IEnumerable<int>>
            GetRecordedAppointmentIdsAsync();

        Task<HealthRecord>
            GetByAppointmentIdAsync(
                int appointmentId);

        Task<IEnumerable<HealthRecord>>
            GetAllRecordsWithDetailsAsync();
    }
}