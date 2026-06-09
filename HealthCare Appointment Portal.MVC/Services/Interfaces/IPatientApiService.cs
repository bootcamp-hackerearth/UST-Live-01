using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal_MVC.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string searchTerm = null);

        Task<PatientDto> GetPatientByIdAsync(int id);

        Task<PatientDto> GetPatientByEmailAsync(string email);

        Task<int> CreatePatientAsync(CreatePatientDto dto);

        Task UpdatePatientAsync(int id, UpdatePatientDto dto);

        Task DeletePatientAsync(int id);
    }
}