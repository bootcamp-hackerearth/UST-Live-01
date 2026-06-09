using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IHealthRecordRepository
    {
        Task<HealthRecord>
            GetByIdAsync(
                int id);

        Task<IEnumerable<HealthRecord>>
            GetAllAsync();

        Task AddAsync(
            HealthRecord healthRecord);

        Task UpdateAsync(
            HealthRecord healthRecord);

        Task DeleteAsync(
            int id);

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