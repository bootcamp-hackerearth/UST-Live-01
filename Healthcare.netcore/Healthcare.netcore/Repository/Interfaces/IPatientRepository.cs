using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByUserIdAsync(int userId);
}