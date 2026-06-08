using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>>
            GetAllHealthRecordsAsync();

        Task<HealthRecordDto>
            GetHealthRecordByIdAsync(
                int recordId);

        Task<IEnumerable<HealthRecordDto>>
            GetRecordsByPatientAsync(
                int patientId);

        Task<IEnumerable<HealthRecordDto>>
            GetRecordsByDoctorAsync(
                int doctorId);

        Task<int>
            AddHealthRecordAsync(
                CreateHealthRecordDto dto);

        Task UpdateHealthRecordAsync(
            int recordId,
            UpdateHealthRecordDto dto);

        Task DeleteHealthRecordAsync(
            int recordId);
    }
}