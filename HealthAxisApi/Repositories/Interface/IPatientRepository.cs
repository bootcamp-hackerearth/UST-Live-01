using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<IEnumerable<Patient>> SearchPatients(string? name, string? email);

        Task<bool> IsDuplicate(string email, string phoneNumber);
    }
}
