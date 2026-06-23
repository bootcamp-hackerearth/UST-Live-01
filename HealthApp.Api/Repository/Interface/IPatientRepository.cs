using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<Patient?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cd = default);
    }
}
