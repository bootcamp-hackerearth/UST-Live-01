using HealthCare_Appointment_Portal.DTOs.HealthRecordDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IHealthRecordApiService
    {
        Task<IEnumerable<HealthRecordDto>>
            GetAllHealthRecordsAsync();

        Task<HealthRecordDto>
            GetHealthRecordByIdAsync(
                int id);

        Task<IEnumerable<HealthRecordDto>>
            GetRecordsByPatientAsync(
                int patientId);

        Task<IEnumerable<HealthRecordDto>>
            GetRecordsByDoctorAsync(
                int doctorId);

        Task<int>
            CreateHealthRecordAsync(
                CreateHealthRecordDto dto);

        Task
            UpdateHealthRecordAsync(
                int id,
                UpdateHealthRecordDto dto);

        Task
            DeleteHealthRecordAsync(
                int id);
    }
}