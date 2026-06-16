using HealthCare.Api.Models;

namespace HealthCare.Api.Repositories.Interfaces
{
    public interface IPatientRepository : IRepository<Patient>
    {
       // Task<PagedResult<Patient>> GetPaginatedPatientsAsync(string searchTerm, int pageNumber, int pageSize);
       
    }
}
