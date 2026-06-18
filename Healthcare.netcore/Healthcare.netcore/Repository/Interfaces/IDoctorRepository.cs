using HealthAxis.API.Models;

namespace HealthAxis.API.Repositories.Interfaces
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> SearchByNameAsync(string name);

        Task<IEnumerable<Doctor>> GetBySpecialisationAsync(string specialization);

        Task<IEnumerable<Doctor>> GetAvailableDoctorsAsync();
    }
}
