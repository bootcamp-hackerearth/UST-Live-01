using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();

        Task<Patient> GetByIdAsync(int id);

        Task AddAsync(Patient patient);

        Task UpdateAsync(Patient patient);

        Task<bool> EmailExistsAsync(string email);

        Task<bool> EmailExistsForOtherPatientAsync(
            int patientId,
            string email);
    }
}