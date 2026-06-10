using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> 
            GetAllPatientsAsync(string searchTerm = null);

        Task<PatientDto>
            GetPatientByIdAsync(
                int patientId);

        Task<PatientDto>
            GetPatientByEmailAsync(
                string email);

        Task<int>
            AddPatientAsync(
                CreatePatientDto patientDto);

        Task UpdatePatientAsync(
            int patientId,
            UpdatePatientDto patientDto);

        Task DeletePatientAsync(
            int patientId);
    }
}