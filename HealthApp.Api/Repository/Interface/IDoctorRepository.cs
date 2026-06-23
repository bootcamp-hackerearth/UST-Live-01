using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<List<Doctor>?> searchbyspecialisationAsync(string specialisation);

        Task<List<Doctor>> getAllActiveAsync();

        Task<Doctor?> GetByIdentityUserIdAsync(string identityUserId);

    }
}

