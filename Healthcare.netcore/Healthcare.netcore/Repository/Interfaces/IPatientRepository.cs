using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetByEmailAsync(string email);

        Task<Patient?> GetByPhoneAsync(string phone);

        Task<IEnumerable<Patient>> SearchByNameAsync(string name);
    }
}