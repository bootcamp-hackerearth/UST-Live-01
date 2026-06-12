using HealthApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Interface
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetAllAsync();

        Task<Patient> GetByIdAsync(int id);

        Task AddAsync(Patient patient);

        Task UpdatePatientAsync(int id, Patient patient);
    }
}