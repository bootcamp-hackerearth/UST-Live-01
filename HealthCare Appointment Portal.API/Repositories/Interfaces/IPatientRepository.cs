using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> GetByIdAsync(int id);

        Task<IEnumerable<Patient>> GetAllAsync();

        Task AddAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task DeleteAsync(int id);

        Task<Patient> GetPatientByEmailAsync(string email);

        Task<IEnumerable<Patient>> GetPatientsByInsuranceStatusAsync(
            InsuranceStatus status);
    }
}