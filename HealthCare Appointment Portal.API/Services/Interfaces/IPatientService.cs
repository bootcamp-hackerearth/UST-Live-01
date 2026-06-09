using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string searchTerm = null);

        Task<PatientDto> GetPatientByIdAsync(int patientId);

        Task<PatientDto> GetPatientByEmailAsync(string email);

        Task<int> AddPatientAsync(CreatePatientDto patientDto);

        Task UpdatePatientAsync(int patientId, UpdatePatientDto patientDto);

        Task DeletePatientAsync(int patientId);
    }
}