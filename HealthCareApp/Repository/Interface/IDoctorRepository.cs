using HealthCareApp.Models;

using HealthCareApp.Enums;

namespace HealthCareApp.Repository.Interface
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<List<Doctor>> GetAllActiveAsync(CancellationToken ct = default);

        Task<List<Doctor>> GetBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default);

        Task<List<Doctor>> GetActiveBySpecialisationAsync(SpecialisationType specialisation, CancellationToken ct = default);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);

    }
}