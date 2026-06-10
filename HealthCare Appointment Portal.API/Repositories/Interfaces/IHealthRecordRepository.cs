using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IHealthRecordRepository
    {
        Task<IEnumerable<HealthRecord>>
            GetAllAsync();

        Task<HealthRecord>
            GetByIdAsync(
                int recordId);

        Task<IEnumerable<HealthRecord>>
            GetByPatientAsync(
                int patientId);

        Task<IEnumerable<HealthRecord>>
            GetByDoctorAsync(
                int doctorId);

        Task<bool>
            ExistsByAppointmentAsync(
                int appointmentId);

        Task
            AddAsync(
                HealthRecord record);

        Task
            UpdateAsync(
                HealthRecord record);

        Task
            DeleteAsync(
                int recordId);
    }
}