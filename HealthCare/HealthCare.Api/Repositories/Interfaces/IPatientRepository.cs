using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Models;

namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
       Task<Patient>GetByUserIdAsync(string userId);
    }
}
