using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<IEnumerable<PatientDto>>
            GetAllPatientsAsync();

        Task<PatientDto>
            GetPatientByIdAsync(
                int id);

        Task<PatientDto>
            GetPatientByEmailAsync(
                string email);

        Task<IEnumerable<PatientDto>>
            GetPatientsByInsuranceStatusAsync(
                InsuranceStatus status);

        Task<int>
            CreatePatientAsync(
                CreatePatientDto dto);

        Task
            UpdatePatientAsync(
                int id,
                UpdatePatientDto dto);

        Task
            DeletePatientAsync(
                int id);
    }
}