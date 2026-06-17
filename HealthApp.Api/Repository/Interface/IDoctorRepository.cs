using HealthApp.Api.Model;

namespace HealthApp.Api.Repository.Interface
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Task<Doctor?> searchbyspecialisationAsync(string specialisation);
    }
}
